namespace KSociety.Log.Serilog.Sinks.RichTextBoxQueue.Wpf.Sinks.RichTextBoxQueue;
using KSociety.Log.Serilog.Sinks.RichTextBox.Wpf.Shared.Sinks.RichTextBox.Abstraction;
using KSociety.Log.Serilog.Sinks.RichTextBox.Wpf.Shared.Sinks.RichTextBox.Output;
using KSociety.Log.Serilog.Sinks.RichTextBox.Wpf.Shared.Sinks.RichTextBox.Themes;
using global::Serilog.Events;
using global::Serilog.Formatting;
using global::Serilog.Sinks.PeriodicBatching;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using System.Timers;
using System.Windows.Threading;

public sealed class RichTextBoxQueueSink : IRichTextBoxQueueSink, IBatchedLogEventSink, IDisposable
{
    private const string DefaultRichTextBoxOutputTemplate = "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}";
    private static readonly object DefaultSyncRoot = new();

    private IRichTextBox? _richTextBox;
    private readonly BufferBlock<LogEvent> _queue;
    private readonly string _outputTemplate;
    private ITextFormatter _formatter;
    private DispatcherPriority _dispatcherPriority;
    private object _syncRoot;
    private RenderAction _renderAction;
    private readonly System.Timers.Timer _timer;
    private readonly SemaphoreSlim _processQueueLock = new SemaphoreSlim(1, 1);

    public RichTextBoxQueueSink(string outputTemplate = DefaultRichTextBoxOutputTemplate)
    {
        this._queue = new BufferBlock<LogEvent>();
        this._outputTemplate = outputTemplate;
        this._timer = new System.Timers.Timer();
        this._timer.Elapsed += this.TimerOnElapsed;
        this._timer.Interval = 300;
        
    }

    private async void TimerOnElapsed(object? sender, ElapsedEventArgs e)
    {
        await this._processQueueLock.WaitAsync().ConfigureAwait(false);
        try
        {
            this._timer.Stop();

            await this.ProcessQueue().ConfigureAwait(false);

            this._timer.Start();
        }
        finally
        {
            this._processQueueLock.Release();
        }
    }

    public void AddRichTextBox(System.Windows.Controls.RichTextBox richTextBoxControl, DispatcherPriority dispatcherPriority = DispatcherPriority.Background, IFormatProvider? formatProvider = null, RichTextBoxTheme? theme = null, object? syncRoot = null)
    {
        var appliedTheme = theme ?? RichTextBoxConsoleThemes.Literate;

        this._formatter = new XamlOutputTemplateRenderer(appliedTheme, this._outputTemplate, formatProvider);

        this._richTextBox = new Serilog.Sinks.RichTextBox.Wpf.Shared.Sinks.RichTextBox.Abstraction.RichTextBox(richTextBoxControl);

        if (!Enum.IsDefined(typeof(DispatcherPriority), dispatcherPriority))
        {
            throw new InvalidEnumArgumentException(nameof(dispatcherPriority), (int)dispatcherPriority,
                typeof(DispatcherPriority));
        }

        this._dispatcherPriority = dispatcherPriority;
        this._syncRoot = syncRoot ?? DefaultSyncRoot;
        this._renderAction = this.Render;

        this._timer.Start();
    }

    private async Task ProcessQueue(CancellationToken cancellationToken = default)
    {
        StringBuilder sb = new();
            
        if (await this._queue.OutputAvailableAsync(cancellationToken).ConfigureAwait(false))
        {
            while (this._queue.TryReceive(null, out var logEvent))
            {
                try
                {
                    sb.Append(
                        $"<Paragraph xmlns =\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" xml:space=\"preserve\">");
                    StringWriter writer = new();
                    this._formatter.Format(logEvent, writer);
                    sb.Append(writer);

                    sb.Append("</Paragraph>");
                    var xamlParagraphText = sb.ToString();

                    lock (this._syncRoot)
                    {
                        this._richTextBox?.BeginInvoke(this._dispatcherPriority, this._renderAction, xamlParagraphText);
                    }
                }
                catch (Exception)
                {
                    //Console.WriteLine("ProcessQueue: {0}", ex.Message);
                }finally
                {
                    sb.Clear();
                }
            }
        }
    }

    private void Render(string xamlParagraphText)
    {
        lock (this._syncRoot)
        {
            try
            {
                this._richTextBox?.Write(xamlParagraphText);
            }
            catch (Exception)
            {
                //Console.WriteLine("Render: {0}", xamlParagraphText);
            }
        }
    }

    public void Dispose()
    {
        this._queue.Complete();
        this._timer.Close();
    }

    private delegate void RenderAction(string xamlParagraphText);

    public async Task EmitBatchAsync(IEnumerable<LogEvent> batch)
    {
        if (batch.Any())
        {
            foreach (var logEvent in batch)
            {
                await this._queue.SendAsync(logEvent).ConfigureAwait(false);
            }
        }
    }

    public Task OnEmptyBatchAsync()
    {
        return Task.CompletedTask;
    }
}
