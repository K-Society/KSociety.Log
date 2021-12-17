﻿using KSociety.Base.EventBus;
using KSociety.Base.EventBus.Abstractions.EventBus;
using KSociety.Base.EventBusRabbitMQ;
using KSociety.Log.Biz.IntegrationEvent.Event;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using Serilog.Events;
using Serilog.Formatting;
using Serilog.Sinks.PeriodicBatching;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace KSociety.Log.Serilog.Sinks.RabbitMq.Sinks.RabbitMq;

/// <summary>
/// Serilog RabbitMq Sink - Lets you log to RabbitMq using Serilog
/// </summary>
public class RabbitMqSink : PeriodicBatchingSink //, IAsyncInitialization
{
    private readonly ITextFormatter _formatter;

    private readonly IConnectionFactory _connectionFactory;
    private readonly IEventBusParameters _eventBusParameters;

    private ILoggerFactory _loggerFactory { get; }
    private IRabbitMqPersistentConnection _persistentConnection { get; set; }
    private Lazy<IEventBus> _eventBus;

    public RabbitMqSink(
        IConnectionFactory connectionFactory,
        IEventBusParameters eventBusParameters,
        RabbitMqSinkConfiguration rabbitMqSinkConfiguration) 
        : base(rabbitMqSinkConfiguration.BatchPostingLimit, rabbitMqSinkConfiguration.Period)
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

        _eventBus = new Lazy<IEventBus>(new EventBusRabbitMqTyped(
            _persistentConnection,
            _loggerFactory,
            null,
            _eventBusParameters,
            "LogQueueDriver", CancellationToken.None));
        //Initialization = InitializeAsync();
        //Initialize();
    }

    //private async ValueTask InitializeAsync()
    //{
    //    try
    //    {
                

    //        _eventBus = new EventBusRabbitMqTyped(
    //            _persistentConnection,
    //            _loggerFactory,
    //            null,
    //            _exchangeDeclareParameters, _queueDeclareParameters,
    //            "LogQueueDriver", CancellationToken.None);

    //        //await _eventBus.Initialization;
    //    }
    //    catch (Exception ex)
    //    {
    //        Console.WriteLine("RabbitMqSink InitializeAsync: " + ex.Message + " " + ex.StackTrace);
    //    }
            
    //}

    protected override async Task EmitBatchAsync(IEnumerable<LogEvent> events)
    {
        foreach (var logEvent in events)
        {
            var sw = new StringWriter();
            _formatter.Format(logEvent, sw);

            var loggerName = "Default";
            if (logEvent.Properties.TryGetValue(global::Serilog.Core.Constants.SourceContextPropertyName, out LogEventPropertyValue sourceContext))
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

    protected override void Dispose(bool disposing)
    {
        // base.Dispose must be called first, because it flushes all pending EmitBatchAsync.
        // Closing the client first would have resulted in an infinite retry loop to flush.
        base.Dispose(disposing);

        try
        {
            // Disposing channel and connection objects is not enough, they must be explicitly closed with the API methods.
            // https://www.rabbitmq.com/dotnet-api-guide.html#disconnecting
            //_client.Close(); //ToDo
        }
        catch
        {
            // ignore exceptions
        }

        //_client.Dispose(); //ToDo
    }
}