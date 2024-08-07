// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace KSociety.Log.Serilog.Sinks.RichTextBox.Wpf
{
    using KSociety.Log.Serilog.Sinks.RichTextBox.Wpf.Sinks.RichTextBox;
    using global::Serilog;
    using global::Serilog.Configuration;
    using global::Serilog.Core;
    using global::Serilog.Events;
    using global::Serilog.Sinks.PeriodicBatching;
    using System;
    using System.Windows.Threading;
    using KSociety.Log.Serilog.Sinks.RichTextBox.Wpf.Shared.Sinks.RichTextBox.Abstraction;
    using KSociety.Log.Serilog.Sinks.RichTextBox.Wpf.Shared.Sinks.RichTextBox.Output;
    using KSociety.Log.Serilog.Sinks.RichTextBox.Wpf.Shared.Sinks.RichTextBox.Themes;
    using RichTextBox = System.Windows.Controls.RichTextBox;

    /// <summary>
    /// Adds the WriteTo.RichTextBox() extension method to <see cref="LoggerConfiguration"/>.
    /// </summary>
    public static class RichTextBoxSinkLoggerConfigurationExtensions
    {
        private static readonly object DefaultSyncRoot = new();
        private const string DefaultRichTextBoxOutputTemplate = "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}";

        private const int DefaultBatchPostingLimit = 500;
        private static readonly TimeSpan DefaultPeriod = TimeSpan.FromMilliseconds(200);

        /// <summary>
        /// Writes log events to a <see cref="System.Windows.Controls.RichTextBox"/> control.
        /// </summary>
        /// <param name="sinkConfiguration">Logger sink configuration.</param>
        /// <param name="richTextBoxControl">The RichTextBox control to write to.</param>
        /// <param name="restrictedToMinimumLevel">The minimum level for
        /// events passed through the sink. Ignored when <paramref name="levelSwitch"/> is specified.</param>
        /// <param name="outputTemplate">A message template describing the format used to write to the sink.
        /// The default is <code>"[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}"</code>.</param>
        /// <param name="formatProvider">Supplies culture-specific formatting information, or null.</param>
        /// <param name="levelSwitch">A switch allowing the pass-through minimum level
        /// to be changed at runtime.</param>
        /// <param name="theme">The theme to apply to the styled output. If not specified,
        /// uses <see cref="RichTextBoxConsoleTheme.Literate"/>.</param>
        /// <param name="dispatcherPriority">The priority at which messages will be sent to the UI thread when logging from a non-UI thread.</param>
        /// <param name="syncRoot">An object that will be used to `lock` (sync) access to the <see cref="IRichTextBox"/>. If you specify this, you
        /// will have the ability to lock on this object, and guarantee that the RichTextBox sink will not be about to output anything while
        /// the lock is held.</param>
        /// <returns>Configuration object allowing method chaining.</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="sinkConfiguration"/> is <code>null</code></exception>
        /// <exception cref="ArgumentNullException">When <paramref name="outputTemplate"/> is <code>null</code></exception>
        public static LoggerConfiguration RichTextBox(
            this LoggerSinkConfiguration sinkConfiguration,
            RichTextBox? richTextBoxControl,
            LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum,
            string outputTemplate = DefaultRichTextBoxOutputTemplate,
            IFormatProvider? formatProvider = null,
            LoggingLevelSwitch? levelSwitch = null,
            RichTextBoxTheme? theme = null, 
            DispatcherPriority dispatcherPriority = DispatcherPriority.Background,
            object? syncRoot = null)
        {
            if (sinkConfiguration is null)
            {
                throw new ArgumentNullException(nameof(sinkConfiguration));
            }

            if (richTextBoxControl is null)
            {
                throw new ArgumentNullException(nameof(richTextBoxControl));
            }

            if (outputTemplate is null)
            {
                throw new ArgumentNullException(nameof(outputTemplate));
            }

            var appliedTheme = theme ?? RichTextBoxConsoleThemes.Literate;

            syncRoot ??= DefaultSyncRoot;

            var formatter = new XamlOutputTemplateRenderer(appliedTheme, outputTemplate, formatProvider);

            var richTextBox = new Shared.Sinks.RichTextBox.Abstraction.RichTextBox(richTextBoxControl, formatter, dispatcherPriority, syncRoot);

            var richTextBoxSink = new RichTextBoxSink(richTextBox, formatter, dispatcherPriority, syncRoot);

            var periodicBatchingSinkOptions = new PeriodicBatchingSinkOptions
            {
                BatchSizeLimit = DefaultBatchPostingLimit,
                Period = DefaultPeriod,
                EagerlyEmitFirstEvent = true,
                QueueLimit = 10000
            };

            var periodicBatchingSink = new PeriodicBatchingSink(richTextBoxSink, periodicBatchingSinkOptions);

            return sinkConfiguration.Sink(periodicBatchingSink, restrictedToMinimumLevel, levelSwitch);
        }

        /// <summary>
        /// Writes log events to an <see cref="IRichTextBox"/> control, used only for unit-testing purposes.
        /// </summary>
        /// <param name="sinkConfiguration">Logger sink configuration.</param>
        /// <param name="richTextBox">The RichTextBox control to write to.</param>
        /// <param name="restrictedToMinimumLevel">The minimum level for
        /// events passed through the sink. Ignored when <paramref name="levelSwitch"/> is specified.</param>
        /// <param name="outputTemplate">A message template describing the format used to write to the sink.
        /// The default is <code>"[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}"</code>.</param>
        /// <param name="formatProvider">Supplies culture-specific formatting information, or null.</param>
        /// <param name="levelSwitch">A switch allowing the pass-through minimum level
        /// to be changed at runtime.</param>
        /// <param name="theme">The theme to apply to the styled output. If not specified,
        /// uses <see cref="RichTextBoxConsoleTheme.Literate"/>.</param>
        /// <param name="dispatcherPriority">The priority at which messages will be sent to the UI thread when logging from a non-UI thread.</param>
        /// <param name="syncRoot">An object that will be used to `lock` (sync) access to the <see cref="IRichTextBox"/> instance. If you specify this, you
        /// will have the ability to lock on this object, and guarantee that the RichTextBox sink will not be about to output anything while
        /// the lock is held.</param>
        /// <returns>Configuration object allowing method chaining.</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="sinkConfiguration"/> is <code>null</code></exception>
        /// <exception cref="ArgumentNullException">When <paramref name="outputTemplate"/> is <code>null</code></exception>
        internal static LoggerConfiguration RichTextBox(
            this LoggerSinkConfiguration sinkConfiguration,
            IRichTextBox richTextBox,
            LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum,
            string outputTemplate = DefaultRichTextBoxOutputTemplate,
            IFormatProvider? formatProvider = null,
            LoggingLevelSwitch? levelSwitch = null,
            RichTextBoxTheme? theme = null,
            DispatcherPriority dispatcherPriority = DispatcherPriority.Background,
            object? syncRoot = null)
        {
            if (sinkConfiguration is null)
            {
                throw new ArgumentNullException(nameof(sinkConfiguration));
            }

            if (outputTemplate is null)
            {
                throw new ArgumentNullException(nameof(outputTemplate));
            }

            var appliedTheme = theme ?? RichTextBoxConsoleThemes.Literate;

            syncRoot ??= DefaultSyncRoot;

            var formatter = new XamlOutputTemplateRenderer(appliedTheme, outputTemplate, formatProvider);

            var richTextBoxSink = new RichTextBoxSink(richTextBox, formatter, dispatcherPriority, syncRoot);

            var periodicBatchingSinkOptions = new PeriodicBatchingSinkOptions
            {
                BatchSizeLimit = DefaultBatchPostingLimit,
                Period = DefaultPeriod,
                EagerlyEmitFirstEvent = true,
                QueueLimit = 10000
            };

            var periodicBatchingSink = new PeriodicBatchingSink(richTextBoxSink, periodicBatchingSinkOptions);

            return sinkConfiguration.Sink(periodicBatchingSink, restrictedToMinimumLevel, levelSwitch);
        }
    }
}
