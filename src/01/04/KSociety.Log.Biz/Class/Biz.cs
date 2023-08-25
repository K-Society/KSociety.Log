namespace KSociety.Log.Biz.Class
{
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
            this._loggerFactory = loggerFactory;
            this._logger = this._loggerFactory.CreateLogger<Biz>();
            this._eventBusParameters = eventBusParameters;
            this._persistentConnection = persistentConnection;
            this._logger.LogInformation("KSociety.Log.Biz.Class.Biz!");
            this._subscriber = new Subscriber(this._loggerFactory, this._persistentConnection, this._eventBusParameters);
        }

        public void LoadEventBus()
        {
            this._subscriber.SubscribeTyped<LogEventHandler, WriteLogEvent>(
                EventBusName, "LogQueueServer", "log", new LogEventHandler(this._loggerFactory)
            );
        }

        public bool WriteLog(WriteLogEvent logEvent)
        {
            ((IEventBusTyped)this._subscriber.EventBus[EventBusName]).Publish(logEvent).ConfigureAwait(false);

            return true;
        }

        public bool WriteLogs(IEnumerable<WriteLogEvent> logEvents)
        {
            foreach (var logEvent in logEvents)
            {
                ((IEventBusTyped)this._subscriber.EventBus[EventBusName]).Publish(logEvent).ConfigureAwait(false);
            }

            return true;
        }

        public async ValueTask<bool> WriteLogAsync(WriteLogEvent logEvent)
        {
            await ((IEventBusTyped)this._subscriber.EventBus[EventBusName]).Publish(logEvent).ConfigureAwait(false);

            return true;
        }

        public async ValueTask<bool> WriteLogsAsync(IEnumerable<WriteLogEvent> logEvents)
        {
            foreach (var logEvent in logEvents)
            {
                await ((IEventBusTyped)this._subscriber.EventBus[EventBusName]).Publish(logEvent).ConfigureAwait(false);
            }

            return true;
        }
    }
}
