using KSociety.Base.EventBus;
using KSociety.Base.EventBus.Abstractions.EventBus;
using KSociety.Base.EventBusRabbitMQ;
using KSociety.Log.Biz.Event;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using Serilog.Events;
using Serilog.Formatting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace KSociety.Log.Serilog.Sinks.RabbitMq.Sinks.RabbitMq
{
    public class RabbitMqBatchedSink : IRabbitMqBatchedSink
    {
        private readonly ITextFormatter _formatter;

        private readonly IConnectionFactory _connectionFactory;
        private readonly IEventBusParameters _eventBusParameters;
        private readonly IRabbitMqPersistentConnection _persistentConnection;

        private ILoggerFactory _loggerFactory { get; }

        private Lazy<IEventBusTyped> _eventBus;

        public RabbitMqBatchedSink(
            IConnectionFactory connectionFactory,
            IEventBusParameters eventBusParameters,
            RabbitMqSinkConfiguration rabbitMqSinkConfiguration
        )
        {
            _formatter = rabbitMqSinkConfiguration.TextFormatter;

            _connectionFactory = connectionFactory;
            _eventBusParameters = eventBusParameters;

            _loggerFactory = LoggerFactory.Create(builder =>
            {
                builder
                    .AddFilter("Microsoft", LogLevel.Warning)
                    .AddFilter("System", LogLevel.Warning);
            });
            _persistentConnection = new DefaultRabbitMqPersistentConnection(_connectionFactory, _loggerFactory);
        }

        public void Initialize()
        {
            _eventBus = new Lazy<IEventBusTyped>(new EventBusRabbitMqTyped(
                _persistentConnection,
                _loggerFactory,
                null,
                _eventBusParameters,
                "LogQueueDriver"));
            _eventBus.Value.Initialize();
        }

        public async Task EmitBatchAsync(IEnumerable<LogEvent> batch)
        {
            foreach (var logEvent in batch)
            {
                var sw = new StringWriter();
                _formatter.Format(logEvent, sw);

                var loggerName = "Default";
                if (logEvent.Properties.TryGetValue(global::Serilog.Core.Constants.SourceContextPropertyName,
                        out LogEventPropertyValue sourceContext))
                {
                    var sv = sourceContext as ScalarValue;
                    if (sv?.Value is string value)
                    {
                        loggerName = value;
                    }
                }

                await _eventBus.Value.Publish(new WriteLogEvent(sw.ToString(), logEvent.Timestamp.DateTime, 1,
                    (int)logEvent.Level, loggerName)).ConfigureAwait(false);
            }
        }

        public Task OnEmptyBatchAsync()
        {
            return Task.CompletedTask;
        }
    }
}