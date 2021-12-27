using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using KSociety.Base.EventBus;
using KSociety.Base.EventBus.Abstractions.EventBus;
using KSociety.Base.EventBusRabbitMQ;
using KSociety.Log.Biz.IntegrationEvent.Event;
using KSociety.Log.Biz.IntegrationEvent.EventHandling;
using KSociety.Log.Biz.Interface;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace KSociety.Log.Biz.Class;

public class Biz : IBiz
{
    private readonly ILogger<Biz> _logger;
    private readonly ILoggerFactory _loggerFactory;
    private readonly IConnectionFactory _connectionFactory;
    private readonly IEventBusParameters _eventBusParameters;

    public IRabbitMqPersistentConnection PersistentConnection { get; }
    private IEventBus _eventBus;

    public Biz(
        ILoggerFactory loggerFactory,
        IConnectionFactory connectionFactory,
        IEventBusParameters eventBusParameters)
    {
        _loggerFactory = loggerFactory;
        _logger = _loggerFactory.CreateLogger<Biz>();
        _connectionFactory = connectionFactory;
        _eventBusParameters = eventBusParameters;
        _logger.LogInformation("KSociety.Log.Biz.Class.Biz!");

        PersistentConnection = new DefaultRabbitMqPersistentConnection(_connectionFactory, _loggerFactory);
    }

    public async void LoadEventBus()
    {
        _eventBus = new EventBusRabbitMqTyped(
            PersistentConnection, 
            _loggerFactory, 
            new LogEventHandler(_loggerFactory), null,
            _eventBusParameters,
            "LogQueueServer", CancellationToken.None);

        ((IEventBusTyped)_eventBus).Subscribe<WriteLogEvent, LogEventHandler>("log");
    }

    public bool WriteLog(WriteLogEvent logEvent)
    {
        _eventBus.Publish(logEvent);

        return true;
    }

    public bool WriteLogs(IEnumerable<WriteLogEvent> logEvents)
    {
        foreach (var logEvent in logEvents)
        {
            _eventBus.Publish(logEvent);
        }

        return true;
    }

    public async ValueTask<bool> WriteLogAsync(WriteLogEvent logEvent)
    {
        await _eventBus.Publish(logEvent).ConfigureAwait(false);

        return true;
    }

    public async ValueTask<bool> WriteLogsAsync(IEnumerable<WriteLogEvent> logEvents)
    {
        foreach (var logEvent in logEvents)
        {
            await _eventBus.Publish(logEvent).ConfigureAwait(false);
        }

        return true;
    }
}