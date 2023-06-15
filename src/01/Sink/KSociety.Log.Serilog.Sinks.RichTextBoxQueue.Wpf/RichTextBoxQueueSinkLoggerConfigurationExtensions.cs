using KSociety.Log.Serilog.Sinks.RichTextBoxQueue.Wpf.Sinks.RichTextBoxQueue;
using Serilog;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;
using Serilog.Sinks.PeriodicBatching;
using System;

namespace KSociety.Log.Serilog.Sinks.RichTextBoxQueue.Wpf
{
    public static class RichTextBoxQueueSinkLoggerConfigurationExtensions
    {
        private const string DefaultRichTextBoxOutputTemplate = "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}";

        private const int DefaultBatchPostingLimit = 500;
        private static readonly TimeSpan DefaultPeriod = TimeSpan.FromMilliseconds(200);

        public static IRichTextBoxQueueSink? RichTextBoxQueueSink { get; private set; }

        ///// <summary>
        ///// Writes log events to a <see cref="System.Windows.Controls.RichTextBox"/> control.
        ///// </summary>
        ///// <param name="sinkConfiguration">Logger sink configuration.</param>
        ///// <param name="restrictedToMinimumLevel">The minimum level for
        ///// events passed through the sink. Ignored when <paramref name="levelSwitch"/> is specified.</param>
        ///// <param name="outputTemplate">A message template describing the format used to write to the sink.
        ///// The default is <code>"[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}"</code>.</param>
        ///// <param name="formatProvider">Supplies culture-specific formatting information, or null.</param>
        ///// <param name="levelSwitch">A switch allowing the pass-through minimum level
        ///// to be changed at runtime.</param>
        ///// <param name="dispatcherPriority">The priority at which messages will be sent to the UI thread when logging from a non-UI thread.</param>
        ///// <param name="syncRoot">An object that will be used to `lock` (sync) access to the <see cref="IRichTextBox"/>. If you specify this, you
        ///// will have the ability to lock on this object, and guarantee that the RichTextBox sink will not be about to output anything while
        ///// the lock is held.</param>
        ///// <returns>Configuration object allowing method chaining.</returns>
        ///// <exception cref="ArgumentNullException">When <paramref name="sinkConfiguration"/> is <code>null</code></exception>
        ///// <exception cref="ArgumentNullException">When <paramref name="outputTemplate"/> is <code>null</code></exception>
        //public static LoggerConfiguration RichTextBox(
        //    this LoggerSinkConfiguration sinkConfiguration,
        //    LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum,
        //    LoggingLevelSwitch? levelSwitch = null)
        //{
        //    if (sinkConfiguration is null)
        //    {
        //        throw new ArgumentNullException(nameof(sinkConfiguration));
        //    }

        //    //syncRoot ??= DefaultSyncRoot;

        //    //var formatter = new XamlOutputTemplateRenderer(appliedTheme, outputTemplate, formatProvider);

        //    //var richTextBox = new RichTextBoxImpl(richTextBoxControl);

        //    var richTextBoxSink = new RichTextBoxQueueSink(/*richTextBox, formatter, dispatcherPriority, syncRoot*/);

        //    var periodicBatchingSinkOptions = new PeriodicBatchingSinkOptions
        //    {
        //        BatchSizeLimit = DefaultBatchPostingLimit,
        //        Period = DefaultPeriod,
        //        EagerlyEmitFirstEvent = true,
        //        QueueLimit = 10000
        //    };

        //    var periodicBatchingSink = new PeriodicBatchingSink(richTextBoxSink, periodicBatchingSinkOptions);

        //    return sinkConfiguration.Sink(periodicBatchingSink, restrictedToMinimumLevel, levelSwitch);
        //}

        /// <summary>
        /// Writes log events to an <see cref="IRichTextBox"/> control, used only for unit-testing purposes.
        /// </summary>
        /// <param name="sinkConfiguration">Logger sink configuration.</param>
        /// <param name="restrictedToMinimumLevel">The minimum level for
        /// events passed through the sink. Ignored when <paramref name="levelSwitch"/> is specified.</param>
        /// <param name="outputTemplate">A message template describing the format used to write to the sink.
        /// The default is <code>"[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}"</code>.</param>
        /// <param name="levelSwitch">A switch allowing the pass-through minimum level
        /// to be changed at runtime.</param>
        /// <returns>Configuration object allowing method chaining.</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="sinkConfiguration"/> is <code>null</code></exception>
        public static LoggerConfiguration RichTextBoxQueue(
            this LoggerSinkConfiguration sinkConfiguration,
            LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum,
            string outputTemplate = DefaultRichTextBoxOutputTemplate,
            LoggingLevelSwitch? levelSwitch = null)
        {
            if (sinkConfiguration is null)
            {
                throw new ArgumentNullException(nameof(sinkConfiguration));
            }

            if (outputTemplate is null)
            {
                throw new ArgumentNullException(nameof(outputTemplate));
            }

            //var appliedTheme = theme ?? RichTextBoxConsoleThemes.Literate;

            //syncRoot ??= DefaultSyncRoot;

            //var formatter = new XamlOutputTemplateRenderer(appliedTheme, outputTemplate, formatProvider);

            var richTextBoxQueueSink = new RichTextBoxQueueSink(outputTemplate /*formatter, dispatcherPriority, syncRoot*/);
            RichTextBoxQueueSink = richTextBoxQueueSink;
            var periodicBatchingSinkOptions = new PeriodicBatchingSinkOptions
            {
                BatchSizeLimit = DefaultBatchPostingLimit,
                Period = DefaultPeriod,
                EagerlyEmitFirstEvent = true,
                QueueLimit = 10000
            };

            var periodicBatchingSink = new PeriodicBatchingSink(richTextBoxQueueSink, periodicBatchingSinkOptions);

            return sinkConfiguration.Sink(periodicBatchingSink, restrictedToMinimumLevel, levelSwitch);
        }
    }
}
