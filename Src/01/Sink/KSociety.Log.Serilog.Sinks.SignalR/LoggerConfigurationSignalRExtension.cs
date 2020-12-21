using System;
using KSociety.Log.Serilog.Sinks.SignalR.Sinks.SignalR;
using Serilog;
using Serilog.Configuration;
using Serilog.Formatting;

namespace KSociety.Log.Serilog.Sinks.SignalR
{
    /// <summary>
    /// Adds the WriteTo.SignalR() extension method to <see cref="LoggerConfiguration"/>.
    /// </summary>
    public static class LoggerConfigurationSignalRExtension
    {
        private const int DefaultBatchPostingLimit = 50;
        private static readonly TimeSpan DefaultPeriod = TimeSpan.FromSeconds(2);

        public static LoggerConfiguration SignalR(
            this LoggerSinkConfiguration loggerConfiguration,
            Action<HubProxy, SignalRSinkConfiguration> configure
            )
        {
            SignalRSinkConfiguration sinkConfiguration = new SignalRSinkConfiguration();
            HubProxy proxy = new HubProxy();

            configure(proxy, sinkConfiguration);

            return RegisterSink(loggerConfiguration, proxy, sinkConfiguration);
        }

        public static LoggerConfiguration SignalR(
            this LoggerSinkConfiguration loggerConfiguration, 
            HubProxy proxy, SignalRSinkConfiguration signalRSinkConfiguration
        )
        {
            //SignalRSinkConfiguration sinkConfiguration = new SignalRSinkConfiguration();
            //HubProxy proxy = new HubProxy();

            //configure(proxy, sinkConfiguration);
            

            //var formatter = new OutputTemplateRenderer(appliedTheme, outputTemplate, formatProvider);
            //MessageTemplateTextFormatter
            return RegisterSink(loggerConfiguration, proxy, signalRSinkConfiguration);
        }

        public static LoggerConfiguration SignalR(
            this LoggerSinkConfiguration loggerConfiguration,
            string uri,
            ITextFormatter textFormatter
        )
        {
            var proxy = new HubProxy(uri);
            var signalRSinkConfiguration = new SignalRSinkConfiguration
            {
                TextFormatter = textFormatter
            };
            
            return RegisterSink(loggerConfiguration, proxy, signalRSinkConfiguration);
        }

        static LoggerConfiguration RegisterSink(
            LoggerSinkConfiguration loggerConfiguration,
            HubProxy proxy,
            SignalRSinkConfiguration signalRSinkConfiguration)
        {
            if (loggerConfiguration == null) throw new ArgumentNullException(nameof(loggerConfiguration));
            if (string.IsNullOrEmpty(proxy.Uri)) throw new ArgumentException("uri cannot be 'null' or and empty string.");

            signalRSinkConfiguration.BatchPostingLimit = (signalRSinkConfiguration.BatchPostingLimit == default) ? DefaultBatchPostingLimit : signalRSinkConfiguration.BatchPostingLimit;
            signalRSinkConfiguration.Period = (signalRSinkConfiguration.Period == default) ? DefaultPeriod : signalRSinkConfiguration.Period;

            return
                loggerConfiguration
                    .Sink(new SignalRSink(
                        signalRSinkConfiguration, proxy), signalRSinkConfiguration.RestrictedToMinimumLevel);
        }
    }
}
