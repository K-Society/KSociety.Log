// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace KSociety.Log.EventBus
{
    using KSociety.Base.EventBusRabbitMQ.Binding;
    using RabbitMQ.Client;

    public class MessageBroker : MessageBroker<
        IExchangeLogDeclareParameters,
        IQueueLogDeclareParameters,
        IEventBusLogParameters,
        IConnectionFactory,
        ILogSubscriber,
        ExchangeLogDeclareParameters,
        QueueLogDeclareParameters,
        EventBusLogParameters,
        LogSubscriber>
    {
        public MessageBroker(
            int eventBusNumber,
            string brokerName, Base.EventBus.ExchangeType exchangeType,
            bool exchangeDurable, bool exchangeAutoDelete,
            string mqHostName, string mqUserName, string mqPassword, bool debug,
            bool purgeQueue,
            bool queueDurable,
            bool queueExclusive,
            bool queueAutoDelete) : base(eventBusNumber, brokerName, exchangeType, exchangeDurable, exchangeAutoDelete, mqHostName, mqUserName, mqPassword, debug, purgeQueue, queueDurable, queueExclusive, queueAutoDelete)
        {

        }

        public MessageBroker(MessageBrokerOptions messageBroker, bool debug = false) : base(messageBroker, debug)
        {

        }
    }
}
