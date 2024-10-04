// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace KSociety.Log.Serilog.Sinks.RichTextBoxQueue.Wpf.Sinks.RichTextBoxQueue
{
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
        //private const string DefaultRichTextBoxOutputTemplate = "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{Exception}";
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

            this._observable = this._queue.AsObservable();
        }

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
        }

        public void Dispose()
        {
            if (this._richTextBox != null)
            {
                this._richTextBox?.StopRichTextBoxLimiter();
            }
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
            }
        }

        public Task OnEmptyBatchAsync()
        {
            return Task.CompletedTask;
        }
    }
}
