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
            IEventBusLogParameters eventBusParameters, int eventBusNumber, bool dispatchConsumersAsync) : base(loggerFactory, connectionFactory, eventBusParameters, eventBusNumber, dispatchConsumersAsync)
        {

        }

        public LogSubscriber(
            IConnectionFactory connectionFactory,
            IEventBusLogParameters eventBusParameters,
            int eventBusNumber, bool dispatchConsumersAsync,
            ILogger<EventBusRabbitMq> loggerEventBusRabbitMq = default,
            ILogger<DefaultRabbitMqPersistentConnection> loggerDefaultRabbitMqPersistentConnection = default) : base(connectionFactory, eventBusParameters, eventBusNumber, dispatchConsumersAsync, loggerEventBusRabbitMq, loggerDefaultRabbitMqPersistentConnection)
        {

        }

        public LogSubscriber(
            ILoggerFactory loggerFactory,
            IRabbitMqPersistentConnection persistentConnection,
            IEventBusLogParameters eventBusParameters, int eventBusNumber, bool dispatchConsumersAsync) : base(loggerFactory, persistentConnection, eventBusParameters, eventBusNumber, dispatchConsumersAsync)
        {

        }

        public LogSubscriber(
            IRabbitMqPersistentConnection persistentConnection,
            IEventBusLogParameters eventBusParameters, int eventBusNumber, bool dispatchConsumersAsync,
            ILogger<EventBusRabbitMq> loggerEventBusRabbitMq = default) : base(persistentConnection, eventBusParameters, eventBusNumber, dispatchConsumersAsync, loggerEventBusRabbitMq)
        {

        }
    }
}
