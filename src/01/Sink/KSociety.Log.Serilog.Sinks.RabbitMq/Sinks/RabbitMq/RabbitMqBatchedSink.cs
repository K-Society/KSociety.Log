// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace KSociety.Log.Serilog.Sinks.RabbitMq.Sinks.RabbitMq
{
    using KSociety.Base.EventBus;
    using KSociety.Base.EventBus.Abstractions.EventBus;
    using KSociety.Base.EventBusRabbitMQ;
    using KSociety.Log.Biz.Event;
    using Microsoft.Extensions.Logging;
    using RabbitMQ.Client;
    using global::Serilog.Events;
    using global::Serilog.Formatting;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;

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
            this._formatter = rabbitMqSinkConfiguration.TextFormatter;

            this._connectionFactory = connectionFactory;
            this._eventBusParameters = eventBusParameters;

            this._loggerFactory = LoggerFactory.Create(builder =>
            {
                builder
                    .AddFilter("Microsoft", LogLevel.Warning)
                    .AddFilter("System", LogLevel.Warning);
            });
            this._persistentConnection = new DefaultRabbitMqPersistentConnection(this._connectionFactory, this._loggerFactory);
        }

        public void Initialize()
        {
            this._eventBus = new Lazy<IEventBusTyped>(this.GetEventBusTyped);
            this._eventBus.Value.Initialize<WriteLogEvent>();
        }

        private IEventBusTyped GetEventBusTyped()
        {
            var eventBus = new EventBusRabbitMqTyped(
                this._persistentConnection,
                this._loggerFactory,
                null,
                this._eventBusParameters,
                "LogQueueDriver");

            return eventBus;
        }

        public async Task EmitBatchAsync(IEnumerable<LogEvent> batch)
        {
            foreach (var logEvent in batch)
            {
                var sw = new StringWriter();
                this._formatter.Format(logEvent, sw);

                var loggerName = "Default";
                if (logEvent.Properties.TryGetValue(global::Serilog.Core.Constants.SourceContextPropertyName,
                        out var sourceContext))
                {
                    var sv = sourceContext as ScalarValue;
                    if (sv?.Value is string value)
                    {
                        loggerName = value;
                    }
                }

                await this._eventBus.Value.Publish(new WriteLogEvent(sw.ToString(), logEvent.Timestamp.DateTime, 1,
                    (int)logEvent.Level, loggerName)).ConfigureAwait(false);
            }
        }

        public Task OnEmptyBatchAsync()
        {
            return Task.CompletedTask;
        }
    }
}
