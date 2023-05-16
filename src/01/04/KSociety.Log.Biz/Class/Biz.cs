using System.Collections.Generic;
using System.Threading.Tasks;
using KSociety.Base.EventBus;
using KSociety.Base.EventBus.Abstractions.EventBus;
using KSociety.Base.EventBusRabbitMQ;
using KSociety.Base.EventBusRabbitMQ.Helper;
using KSociety.Log.Biz.Event;
using KSociety.Log.Biz.IntegrationEvent.EventHandling;
using KSociety.Log.Biz.Interface;
using Microsoft.Extensions.Logging;

namespace KSociety.Log.Biz.Class
{
    public class Biz : IBiz
    {
        private readonly ILogger<Biz> _logger;
        private readonly ILoggerFactory _loggerFactory;
        private readonly IEventBusParameters _eventBusParameters;
        private readonly IRabbitMqPersistentConnection _persistentConnection;
        private readonly Subscriber _subscriber;
        private const string EventBusName = "Logger";

        public Biz(
            ILoggerFactory loggerFactory,
            IEventBusParameters eventBusParameters,
            IRabbitMqPersistentConnection persistentConnection)
        {
            _loggerFactory = loggerFactory;
            _logger = _loggerFactory.CreateLogger<Biz>();
            _eventBusParameters = eventBusParameters;
            _persistentConnection = persistentConnection;
            _logger.LogInformation("KSociety.Log.Biz.Class.Biz!");
            _subscriber = new Subscriber(_loggerFactory, _persistentConnection, _eventBusParameters);
        }

        public void LoadEventBus()
        {
            _subscriber.SubscribeTyped<LogEventHandler, WriteLogEvent>(
                EventBusName, "LogQueueServer", "log", new LogEventHandler(_loggerFactory)
            );
        }

        public bool WriteLog(WriteLogEvent logEvent)
        {
            ((IEventBusTyped)_subscriber.EventBus[EventBusName]).Publish(logEvent).ConfigureAwait(false);

            return true;
        }

        public bool WriteLogs(IEnumerable<WriteLogEvent> logEvents)
        {
            foreach (var logEvent in logEvents)
            {
                ((IEventBusTyped)_subscriber.EventBus[EventBusName]).Publish(logEvent).ConfigureAwait(false);
            }

            return true;
        }

        public async ValueTask<bool> WriteLogAsync(WriteLogEvent logEvent)
        {
            await ((IEventBusTyped)_subscriber.EventBus[EventBusName]).Publish(logEvent).ConfigureAwait(false);

            return true;
        }

        public async ValueTask<bool> WriteLogsAsync(IEnumerable<WriteLogEvent> logEvents)
        {
            foreach (var logEvent in logEvents)
            {
                await ((IEventBusTyped)_subscriber.EventBus[EventBusName]).Publish(logEvent).ConfigureAwait(false);
            }

            return true;
        }
    }
}