// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace KSociety.Log.EventBus
{
    using KSociety.Base.EventBusRabbitMQ;
    using KSociety.Base.EventBusRabbitMQ.Helper;
    using Microsoft.Extensions.Logging;
    using RabbitMQ.Client;

    public class LogSubscriber : Subscriber, ILogSubscriber
    {
        public LogSubscriber(
            ILoggerFactory loggerFactory,
            IConnectionFactory connectionFactory,
            IEventBusLogParameters eventBusParameters, int eventBusNumber, bool purgeQueue = false)
            : base(loggerFactory, connectionFactory, eventBusParameters, eventBusNumber, purgeQueue)
        {

        }

        public LogSubscriber(
            IConnectionFactory connectionFactory,
            IEventBusLogParameters eventBusParameters,
            int eventBusNumber,
            ILogger<EventBusRabbitMq> loggerEventBusRabbitMq = default,
            ILogger<DefaultRabbitMqPersistentConnection> loggerDefaultRabbitMqPersistentConnection = default, bool purgeQueue = false)
            : base(connectionFactory, eventBusParameters, eventBusNumber, loggerEventBusRabbitMq, loggerDefaultRabbitMqPersistentConnection, purgeQueue)
        {

        }

        public LogSubscriber(
            ILoggerFactory loggerFactory,
            IRabbitMqPersistentConnection persistentConnection,
            IEventBusLogParameters eventBusParameters, int eventBusNumber, bool purgeQueue = false)
            : base(loggerFactory, persistentConnection, eventBusParameters, eventBusNumber, purgeQueue)
        {

        }

        public LogSubscriber(
            IRabbitMqPersistentConnection persistentConnection,
            IEventBusLogParameters eventBusParameters, int eventBusNumber,
            ILogger<EventBusRabbitMq> loggerEventBusRabbitMq = default, bool purgeQueue = false)
            : base(persistentConnection, eventBusParameters, eventBusNumber, loggerEventBusRabbitMq, purgeQueue)
        {

        }
    }
}
