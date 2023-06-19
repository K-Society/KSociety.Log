using KSociety.Log.Serilog.Sinks.RichTextBox.Wpf.Shared.Sinks.RichTextBox.Abstraction;
using KSociety.Log.Serilog.Sinks.RichTextBox.Wpf.Shared.Sinks.RichTextBox.Output;
using KSociety.Log.Serilog.Sinks.RichTextBox.Wpf.Shared.Sinks.RichTextBox.Themes;
using Serilog.Events;
using Serilog.Formatting;
using Serilog.Sinks.PeriodicBatching;
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

namespace KSociety.Log.Serilog.Sinks.RichTextBoxQueue.Wpf.Sinks.RichTextBoxQueue;

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
        _queue = new BufferBlock<LogEvent>();
        _outputTemplate = outputTemplate;
        _timer = new System.Timers.Timer();
        _timer.Elapsed += TimerOnElapsed;
        _timer.Interval = 300;
        
    }

    private async void TimerOnElapsed(object? sender, ElapsedEventArgs e)
    {
        await _processQueueLock.WaitAsync().ConfigureAwait(false);
        try
        {
            _timer.Stop();

            await ProcessQueue().ConfigureAwait(false);

            _timer.Start();
        }
        finally
        {
            _processQueueLock.Release();
        }
    }

    public void AddRichTextBox(System.Windows.Controls.RichTextBox richTextBoxControl, DispatcherPriority dispatcherPriority = DispatcherPriority.Background, IFormatProvider? formatProvider = null, RichTextBoxTheme? theme = null, object? syncRoot = null)
    {
        var appliedTheme = theme ?? RichTextBoxConsoleThemes.Literate;

        _formatter = new XamlOutputTemplateRenderer(appliedTheme, _outputTemplate, formatProvider);

        _richTextBox = new RichTextBox.Wpf.Shared.Sinks.RichTextBox.Abstraction.RichTextBox(richTextBoxControl);

        if (!Enum.IsDefined(typeof(DispatcherPriority), dispatcherPriority))
        {
            throw new InvalidEnumArgumentException(nameof(dispatcherPriority), (int)dispatcherPriority,
                typeof(DispatcherPriority));
        }

        _dispatcherPriority = dispatcherPriority;
        _syncRoot = syncRoot ?? DefaultSyncRoot;
        _renderAction = Render;

        _timer.Start();
    }

    private async Task ProcessQueue(CancellationToken cancellationToken = default)
    {
        StringBuilder sb = new();
            
        if (await _queue.OutputAvailableAsync(cancellationToken).ConfigureAwait(false))
        {
            while (_queue.TryReceive(null, out var logEvent))
            {
                try
                {
                    sb.Append(
                        $"<Paragraph xmlns =\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" xml:space=\"preserve\">");
                    StringWriter writer = new();
                    _formatter.Format(logEvent, writer);
                    sb.Append(writer);

                    sb.Append("</Paragraph>");
                    var xamlParagraphText = sb.ToString();

                    lock (_syncRoot)
                    {
                        _richTextBox?.BeginInvoke(_dispatcherPriority, _renderAction, xamlParagraphText);
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
        lock (_syncRoot)
        {
            try
            {
                _richTextBox?.Write(xamlParagraphText);
            }
            catch (Exception)
            {
                //Console.WriteLine("Render: {0}", xamlParagraphText);
            }
        }
    }

    public void Dispose()
    {
        _queue.Complete();
        _timer.Close();
    }

    private delegate void RenderAction(string xamlParagraphText);

    public async Task EmitBatchAsync(IEnumerable<LogEvent> batch)
    {
        if (batch.Any())
        {
            foreach (var logEvent in batch)
            {
                await _queue.SendAsync(logEvent).ConfigureAwait(false);
            }
        }
    }

    public Task OnEmptyBatchAsync()
    {
        return Task.CompletedTask;
    }
}