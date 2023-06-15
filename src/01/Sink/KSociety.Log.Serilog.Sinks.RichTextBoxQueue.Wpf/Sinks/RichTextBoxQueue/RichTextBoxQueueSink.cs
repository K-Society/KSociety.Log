using Serilog.Events;
using Serilog.Formatting;
using Serilog.Sinks.PeriodicBatching;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using System.Windows.Threading;
using KSociety.Log.Serilog.Sinks.RichTextBox.Wpf.Shared.Sinks.RichTextBox.Abstraction;
using KSociety.Log.Serilog.Sinks.RichTextBox.Wpf.Shared.Sinks.RichTextBox.Event;
using KSociety.Log.Serilog.Sinks.RichTextBox.Wpf.Shared.Sinks.RichTextBox.Output;
using KSociety.Log.Serilog.Sinks.RichTextBox.Wpf.Shared.Sinks.RichTextBox.Themes;

namespace KSociety.Log.Serilog.Sinks.RichTextBoxQueue.Wpf.Sinks.RichTextBoxQueue
{
    public sealed class RichTextBoxQueueSink : IRichTextBoxQueueSink, IBatchedLogEventSink, IDisposable
    {
        private const string DefaultRichTextBoxOutputTemplate = "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}";
        private static readonly object DefaultSyncRoot = new();

        private IRichTextBox? _richTextBox;
        private readonly BufferBlock<LogEvent> _queue;
        private readonly IObservable<LogEvent> _observable;
        //private readonly IObserver<LogEvent> _observer;
        private readonly string _outputTemplate;
        private ITextFormatter _formatter;
        private DispatcherPriority _dispatcherPriority;
        private object _syncRoot;
        private RenderAction _renderAction;

        public RichTextBoxQueueSink(string outputTemplate = DefaultRichTextBoxOutputTemplate)
        {
            _queue = new BufferBlock<LogEvent>();

            _observable = _queue.AsObservable();
            //_observer = _queue.AsObserver();

            _observable.Subscribe(new LogEventHandler());
            //_observer.OnCompleted(XamlOutputTemplateRenderer => ());

            _outputTemplate = outputTemplate;
        }

        public void AddRichTextBox(System.Windows.Controls.RichTextBox richTextBoxControl, DispatcherPriority dispatcherPriority, IFormatProvider? formatProvider = null, RichTextBoxTheme? theme = null, object? syncRoot = null)
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
        }

        private void Render(string xamlParagraphText)
        {
            lock (_syncRoot)
            {
                _richTextBox?.Write(xamlParagraphText);
            }
        }

        public void Dispose()
        {
            _queue.Complete();
        }

        private delegate void RenderAction(string xamlParagraphText);

        public async Task EmitBatchAsync(IEnumerable<LogEvent> batch)
        {
            if (batch.Any())
            {
                //Enumerable.Range(0, 10).Select(x => { x})
                //StringBuilder sb = new();

                foreach (var logEvent in batch)
                {

                    await _queue.SendAsync(logEvent).ConfigureAwait(false);
                    

                    //sb.Append($"<Paragraph xmlns =\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" xml:space=\"preserve\">");
                    //StringWriter writer = new();
                    //_formatter.Format(logEvent, writer);
                    //sb.Append(writer);

                    //sb.Append("</Paragraph>");
                    //string xamlParagraphText = sb.ToString();
                    //lock (_syncRoot)
                    //{
                    //    _richTextBox.BeginInvoke(_dispatcherPriority, _renderAction, xamlParagraphText);
                    //}
                    //sb.Clear();
                }
            }
        }

        public Task OnEmptyBatchAsync()
        {
            return Task.CompletedTask;
        }
    }
}
