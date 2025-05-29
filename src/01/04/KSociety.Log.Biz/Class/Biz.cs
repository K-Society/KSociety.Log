// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace KSociety.Log.Biz.Class
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using KSociety.Base.EventBus.Abstractions.EventBus;
    using KSociety.Log.Biz.Event;
    using KSociety.Log.Biz.IntegrationEvent.EventHandling;
    using KSociety.Log.Biz.Interface;
    using KSociety.Log.EventBus;
    using Microsoft.Extensions.Logging;

    public class Biz : IBiz
    {
        private readonly ILogger<Biz> _logger;
        private readonly ILoggerFactory _loggerFactory;
        private readonly ILogSubscriber _logSubscriber;
        private const string EventBusName = "Logger";

        public Biz(
            ILoggerFactory loggerFactory,
            ILogSubscriber logSubscriber
            )
        {
            this._loggerFactory = loggerFactory;
            this._logger = this._loggerFactory.CreateLogger<Biz>();
            //this._eventBusParameters = eventBusParameters;
            //this._persistentConnection = persistentConnection;
            this._logSubscriber = logSubscriber;
            this._logger.LogInformation("KSociety.Log.Biz.Class.Biz!");
            //this._subscriber = new Subscriber(this._loggerFactory, this._persistentConnection, this._eventBusParameters, 10, true);
        }

        public void LoadEventBus()
        {
            this._logSubscriber.SubscribeTyped<LogEventHandler, WriteLogEvent>(
                EventBusName, "LogQueueServer", "log", new LogEventHandler(this._loggerFactory)
            );
        }

        public bool WriteLog(WriteLogEvent logEvent)
        {
            ((IEventBusTyped)this._logSubscriber.EventBus[EventBusName]).Publish(logEvent).ConfigureAwait(false);

            return true;
        }

        public bool WriteLogs(IEnumerable<WriteLogEvent> logEvents)
        {
            foreach (var logEvent in logEvents)
            {
                ((IEventBusTyped)this._logSubscriber.EventBus[EventBusName]).Publish(logEvent).ConfigureAwait(false);
            }

            return true;
        }

        public async ValueTask<bool> WriteLogAsync(WriteLogEvent logEvent)
        {
            await ((IEventBusTyped)this._logSubscriber.EventBus[EventBusName]).Publish(logEvent).ConfigureAwait(false);

            return true;
        }

        public async ValueTask<bool> WriteLogsAsync(IEnumerable<WriteLogEvent> logEvents)
        {
            foreach (var logEvent in logEvents)
            {
                await ((IEventBusTyped)this._logSubscriber.EventBus[EventBusName]).Publish(logEvent).ConfigureAwait(false);
            }

            return true;
        }
    }
}
