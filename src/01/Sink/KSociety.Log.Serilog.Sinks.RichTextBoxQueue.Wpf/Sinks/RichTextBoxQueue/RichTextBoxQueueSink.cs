// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace KSociety.Log.Serilog.Sinks.RichTextBoxQueue.Wpf.Sinks.RichTextBoxQueue
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Threading.Tasks.Dataflow;
    using System.Windows.Threading;
    using global::Serilog.Events;
    using global::Serilog.Formatting;
    using global::Serilog.Sinks.PeriodicBatching;
    using KSociety.Log.Serilog.Sinks.RichTextBox.Wpf.Shared.Sinks.RichTextBox.Abstraction;
    using KSociety.Log.Serilog.Sinks.RichTextBox.Wpf.Shared.Sinks.RichTextBox.Output;
    using KSociety.Log.Serilog.Sinks.RichTextBox.Wpf.Shared.Sinks.RichTextBox.Themes;

    public sealed class RichTextBoxQueueSink : IRichTextBoxQueueSink, IBatchedLogEventSink, IDisposable, IAsyncDisposable
    {
        private const int DisposedFlag = 1;
        private int _isDisposed;

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

        #region [Dispose]

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly", Justification = "Dispose is implemented correctly, FxCop just doesn't see it.")]
        public void Dispose()
        {
            var wasDisposed = Interlocked.Exchange(ref this._isDisposed, DisposedFlag);
            if (wasDisposed == DisposedFlag)
            {
                return;
            }

            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Free any other managed objects here.
                if (this._richTextBox != null)
                {
                    this._richTextBox.StopRichTextBoxLimiter();
                }

                if (this._observer != null)
                {
                    this._observer.Dispose();
                }

                if (this._queue != null)
                {
                    this._queue.Complete();
                }     
            }

            // Free any unmanaged objects here.
        }

        /// <summary>
        /// Gets a value indicating whether the current instance has been disposed.
        /// </summary>
        public bool IsDisposed
        {
            get
            {
                Interlocked.MemoryBarrier();
                return this._isDisposed == DisposedFlag;
            }
        }

        /// <inheritdoc/>
        [SuppressMessage(
            "Usage",
            "CA1816:Dispose methods should call SuppressFinalize", Justification = "DisposeAsync should also call SuppressFinalize (see various .NET internal implementations).")]
        public ValueTask DisposeAsync()
        {
            // Still need to check if we've already disposed; can't do both.
            var wasDisposed = Interlocked.Exchange(ref this._isDisposed, DisposedFlag);
            if (wasDisposed != DisposedFlag)
            {
                GC.SuppressFinalize(this);

                // Always true, but means we get the similar syntax as Dispose,
                // and separates the two overloads.
                return this.DisposeAsync(true);
            }

            return default;
        }

        /// <summary>
        ///  Releases unmanaged and - optionally - managed resources, asynchronously.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        private ValueTask DisposeAsync(bool disposing)
        {
            // Default implementation does a synchronous dispose.
            this.Dispose(disposing);

            return default;
        }

        #endregion
    }
}
