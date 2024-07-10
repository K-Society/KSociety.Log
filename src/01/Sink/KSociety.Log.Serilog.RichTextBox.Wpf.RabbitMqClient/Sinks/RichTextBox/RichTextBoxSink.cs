// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace KSociety.Log.Serilog.RichTextBox.Wpf.RabbitMqClient.Sinks.RichTextBox
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Threading;
    using KSociety.Log.Serilog.Sinks.RichTextBox.Wpf.Shared.Sinks.RichTextBox.Abstraction;
    using global::Serilog.Events;
    using global::Serilog.Formatting;
    using global::Serilog.Sinks.PeriodicBatching;

    internal sealed class RichTextBoxSink : IBatchedLogEventSink, IDisposable
    {
        private readonly IRichTextBox _richTextBox;
        private readonly ITextFormatter _formatter;
        private readonly DispatcherPriority _dispatcherPriority;
        private readonly object _syncRoot;

        private readonly RenderAction _renderAction;

        public RichTextBoxSink(IRichTextBox richTextBox, ITextFormatter formatter, DispatcherPriority dispatcherPriority, object syncRoot)
        {
            this._richTextBox = richTextBox ?? throw new ArgumentNullException(nameof(richTextBox));
            this._formatter = formatter ?? throw new ArgumentNullException(nameof(formatter));

            if (!Enum.IsDefined(typeof(DispatcherPriority), dispatcherPriority))
            {
                throw new InvalidEnumArgumentException(nameof(dispatcherPriority), (int)dispatcherPriority,
                    typeof(DispatcherPriority));
            }

            this._dispatcherPriority = dispatcherPriority;
            this._syncRoot = syncRoot ?? throw new ArgumentNullException(nameof(syncRoot));

            this._renderAction = this.Render;
        }

        private void Render(string xamlParagraphText)
        {
            //var richTextBox = _richTextBox;

            lock (this._syncRoot)
            {
                //this._richTextBox.Write(xamlParagraphText);
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
                    this._formatter.Format(logEvent, writer);
                    sb.Append(writer);

                    sb.Append("</Paragraph>");
                    var xamlParagraphText = sb.ToString();
                    await this._richTextBox.BeginInvoke(/*this._dispatcherPriority,*/ /*this._renderAction,*/ xamlParagraphText);
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
