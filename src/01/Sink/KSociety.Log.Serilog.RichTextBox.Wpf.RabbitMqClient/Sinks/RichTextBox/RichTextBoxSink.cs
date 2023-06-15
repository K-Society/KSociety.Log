using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using KSociety.Log.Serilog.Sinks.RichTextBox.Wpf.Shared.Sinks.RichTextBox.Abstraction;
using Serilog.Events;
using Serilog.Formatting;
using Serilog.Sinks.PeriodicBatching;

namespace KSociety.Log.Serilog.RichTextBox.Wpf.RabbitMqClient.Sinks.RichTextBox
{
    internal sealed class RichTextBoxSink : IBatchedLogEventSink, IDisposable
    {
        private readonly IRichTextBox _richTextBox;
        private readonly ITextFormatter _formatter;
        private readonly DispatcherPriority _dispatcherPriority;
        private readonly object _syncRoot;

        private readonly RenderAction _renderAction;

        public RichTextBoxSink(IRichTextBox richTextBox, ITextFormatter formatter, DispatcherPriority dispatcherPriority, object syncRoot)
        {
            _richTextBox = richTextBox ?? throw new ArgumentNullException(nameof(richTextBox));
            _formatter = formatter ?? throw new ArgumentNullException(nameof(formatter));

            if (!Enum.IsDefined(typeof(DispatcherPriority), dispatcherPriority))
            {
                throw new InvalidEnumArgumentException(nameof(dispatcherPriority), (int)dispatcherPriority,
                    typeof(DispatcherPriority));
            }

            _dispatcherPriority = dispatcherPriority;
            _syncRoot = syncRoot ?? throw new ArgumentNullException(nameof(syncRoot));

            _renderAction = Render;
        }

        private void Render(string xamlParagraphText)
        {
            //var richTextBox = _richTextBox;

            lock (_syncRoot)
            {
                _richTextBox.Write(xamlParagraphText);
            }
        }

        public void Dispose()
        {
        }

        private delegate void RenderAction(string xamlParagraphText);

        public async Task EmitBatchAsync(IEnumerable<LogEvent> batch)
        {
            if (batch.Any())
            {
                StringBuilder sb = new();
                
                foreach (var logEvent in batch)
                {
                    sb.Append($"<Paragraph xmlns =\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" xml:space=\"preserve\">");
                    StringWriter writer = new();
                    _formatter.Format(logEvent, writer);
                    sb.Append(writer);

                    sb.Append("</Paragraph>");
                    string xamlParagraphText = sb.ToString();
                    await _richTextBox.BeginInvoke(_dispatcherPriority, _renderAction, xamlParagraphText);
                    sb.Clear();
                }
            }
        }

        public Task OnEmptyBatchAsync()
        {
            return Task.CompletedTask;
        }
    }
}
