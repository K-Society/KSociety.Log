// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace KSociety.Log.Serilog.Sinks.RichTextBoxQueue.Wpf
{
    using KSociety.Log.Serilog.Sinks.RichTextBoxQueue.Wpf.Sinks.RichTextBoxQueue;
    using global::Serilog;
    using global::Serilog.Configuration;
    using global::Serilog.Core;
    using global::Serilog.Events;
    using global::Serilog.Sinks.PeriodicBatching;
    using System;

    public static class RichTextBoxQueueSinkLoggerConfigurationExtensions
    {
        private const int DefaultBatchPostingLimit = 500;
        private static readonly TimeSpan DefaultPeriod = TimeSpan.FromMilliseconds(100);
        private static RichTextBoxQueueSink? RichTextBoxQueueSink;

        /// <param name="sinkConfiguration">Logger sink configuration.</param>
        /// <param name="richTextBoxQueueSink"></param>
        /// <param name="restrictedToMinimumLevel">The minimum level for
        /// events passed through the sink. Ignored when <paramref name="levelSwitch"/> is specified.</param>
        /// <param name="levelSwitch">A switch allowing the pass-through minimum level
        /// to be changed at runtime.</param>
        /// <returns>Configuration object allowing method chaining.</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="sinkConfiguration"/> is <code>null</code></exception>
        public static LoggerConfiguration RichTextBoxQueue(
            this LoggerSinkConfiguration sinkConfiguration, RichTextBoxQueueSink? richTextBoxQueueSink = null,
            LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum,
            LoggingLevelSwitch? levelSwitch = null)
        {
            if (sinkConfiguration is null)
            {
                throw new ArgumentNullException(nameof(sinkConfiguration));
            }

            RichTextBoxQueueSink = richTextBoxQueueSink ?? throw new ArgumentNullException(nameof(richTextBoxQueueSink));

            var periodicBatchingSinkOptions = new PeriodicBatchingSinkOptions
            {
                BatchSizeLimit = DefaultBatchPostingLimit,
                Period = DefaultPeriod,
                EagerlyEmitFirstEvent = true,
                QueueLimit = 10000
            };

            var periodicBatchingSink = new PeriodicBatchingSink(RichTextBoxQueueSink, periodicBatchingSinkOptions);

            return sinkConfiguration.Sink(periodicBatchingSink, restrictedToMinimumLevel, levelSwitch);
        }
    }
}
