namespace KSociety.Log.Serilog.Sinks.RichTextBoxQueue.Wpf.Sinks.RichTextBoxQueue;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using System.Windows.Threading;
using global::Serilog.Events;
using global::Serilog.Formatting;
using global::Serilog.Sinks.PeriodicBatching;
using KSociety.Log.Serilog.Sinks.RichTextBox.Wpf.Shared.Sinks.RichTextBox.Abstraction;
using KSociety.Log.Serilog.Sinks.RichTextBox.Wpf.Shared.Sinks.RichTextBox.Output;
using KSociety.Log.Serilog.Sinks.RichTextBox.Wpf.Shared.Sinks.RichTextBox.Themes;

public sealed class RichTextBoxQueueSink : IRichTextBoxQueueSink, IBatchedLogEventSink, IDisposable
{
    private const string DefaultRichTextBoxOutputTemplate = "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}";
    private static readonly object DefaultSyncRoot = new();

    private IRichTextBox? _richTextBox;
    private readonly BufferBlock<LogEvent> _queue;
    private readonly string _outputTemplate;
    private ITextFormatter? _formatter;
    private DispatcherPriority _dispatcherPriority;
    private object _syncRoot;
    private readonly IObservable<LogEvent> _observable;
    private IDisposable _observer;

    public RichTextBoxQueueSink(string outputTemplate = DefaultRichTextBoxOutputTemplate)
    {
        this._queue = new BufferBlock<LogEvent>();
        
        this._outputTemplate = outputTemplate;
        //this._timer = new System.Timers.Timer();
        //this._timer.Elapsed += this.TimerOnElapsed;
        //this._timer.Interval = 300;
        this._observable = this._queue.AsObservable();
    }

    //private async void TimerOnElapsed(object? sender, ElapsedEventArgs e)
    //{
    //    //await this._processQueueLock.WaitAsync().ConfigureAwait(false);
    //    try
    //    {
    //        this._timer.Stop();

    //        await this.ProcessQueue().ConfigureAwait(false);

    //        this._timer.Start();
    //    }
    //    finally
    //    {
    //        //this._processQueueLock.Release();
    //    }
    //}

    public void AddRichTextBox(System.Windows.Controls.RichTextBox? richTextBoxControl, DispatcherPriority dispatcherPriority = DispatcherPriority.Background, IFormatProvider? formatProvider = null, RichTextBoxTheme? theme = null, object? syncRoot = null)
    {
        this._syncRoot = syncRoot ?? DefaultSyncRoot;

        var appliedTheme = theme ?? RichTextBoxConsoleThemes.Literate;

        this._formatter = new XamlOutputTemplateRenderer(appliedTheme, this._outputTemplate, formatProvider);

        if (!Enum.IsDefined(typeof(DispatcherPriority), dispatcherPriority))
        {
            throw new InvalidEnumArgumentException(nameof(dispatcherPriority), (int)dispatcherPriority,
                typeof(DispatcherPriority));
        }

        this._dispatcherPriority = dispatcherPriority;

        this._richTextBox = new Serilog.Sinks.RichTextBox.Wpf.Shared.Sinks.RichTextBox.Abstraction.RichTextBox(richTextBoxControl, this._formatter, this._dispatcherPriority, this._syncRoot);

        this._observer = this._observable.Subscribe(this._richTextBox);

        //this._timer.Start();
    }

    //private async Task ProcessQueue(CancellationToken cancellationToken = default)
    //{
    //    StringBuilder sb = new();

    //    if (await this._queue.OutputAvailableAsync(cancellationToken).ConfigureAwait(false))
    //    {
    //        while (this._queue.TryReceive(null, out var logEvent))
    //        {
    //            try
    //            {
    //                sb.Append($"<Paragraph xmlns =\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" xml:space=\"preserve\">");
    //                StringWriter writer = new();
    //                this._formatter.Format(logEvent, writer);
    //                sb.Append(writer);

    //                sb.Append("</Paragraph>");
    //                var xamlParagraphText = sb.ToString();

    //                await this._richTextBox.BeginInvoke(this._dispatcherPriority, /*this._renderAction,*/ xamlParagraphText).ConfigureAwait(false);
    //            }
    //            catch (Exception)
    //            {
    //                ; //Console.WriteLine("ProcessQueue: {0}", ex.Message);
    //            }
    //            finally
    //            {
    //                sb.Clear();
    //            }
    //        }
    //    }
    //}

    public void Dispose()
    {
        this._queue.Complete();
        this._observer.Dispose();
    }

    public async Task EmitBatchAsync(IEnumerable<LogEvent> batch)
    {
        try
        {
            if (batch.Any())
            {
                foreach (var logEvent in batch)
                {
                    await this._queue.SendAsync(logEvent).ConfigureAwait(false);
                }
            }
        }
        catch (Exception)
        {
            ;
        }
    }

    public Task OnEmptyBatchAsync()
    {
        return Task.CompletedTask;
    }
}
