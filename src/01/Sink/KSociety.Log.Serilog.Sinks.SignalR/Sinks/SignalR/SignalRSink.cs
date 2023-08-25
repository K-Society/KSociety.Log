namespace KSociety.Log.Serilog.Sinks.SignalR.Sinks.SignalR
{
    using Microsoft.Extensions.Logging;
    using global::Serilog.Events;
    using global::Serilog.Formatting;
    using global::Serilog.Sinks.PeriodicBatching;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;

    public class SignalRSink : IBatchedLogEventSink
    {
        private readonly ITextFormatter _formatter;
        private readonly HubProxy _proxy;

        private ILoggerFactory _loggerFactory { get; }

        public SignalRSink(SignalRSinkConfiguration signalRSinkConfiguration, HubProxy proxy)
        {
            this._formatter = signalRSinkConfiguration.TextFormatter;
            this._proxy = proxy;

            this._loggerFactory = LoggerFactory.Create(builder =>
            {
                builder
                    .AddFilter("Microsoft", LogLevel.Warning)
                    .AddFilter("System", LogLevel.Warning);
            });
        }

        public async Task EmitBatchAsync(IEnumerable<LogEvent> events)
        {
            foreach (var logEvent in events)
            {
                var sw = new StringWriter();
                this._formatter.Format(logEvent, sw);

                await this._proxy
                    .Log(new Srv.Dto.LogEvent(sw.ToString(), logEvent.Timestamp.DateTime, 1, (int)logEvent.Level,
                        "LoggerName")).ConfigureAwait(false);
            }
        }

        public Task OnEmptyBatchAsync()
        {
            return Task.CompletedTask;
        }
    }
}