using System;
using KSociety.Base.EventBus;
using KSociety.Log.Serilog.Sinks.RabbitMq.Sinks.RabbitMq;
using RabbitMQ.Client;
using Serilog;
using Serilog.Configuration;
using Serilog.Formatting;

namespace KSociety.Log.Serilog.Sinks.RabbitMq
{
    /// <summary>
    /// Extension method to configure Serilog with a Sink for RabbitMq
    /// </summary>
    public static class LoggerConfigurationRabbitMqExtension
    {
        private const int DefaultBatchPostingLimit = 50;
        private static readonly TimeSpan DefaultPeriod = TimeSpan.FromSeconds(2);

        /// <summary>
        /// Adds a sink that lets you push log messages to RabbitMQ
        /// </summary>
        public static LoggerConfiguration RabbitMq(
            this LoggerSinkConfiguration loggerConfiguration,
            Action<ConnectionFactory, IExchangeDeclareParameters, IQueueDeclareParameters, RabbitMqSinkConfiguration> configure
            /*Action<RabbitMqClientConfiguration, RabbitMqSinkConfiguration> configure*/)
        {
            //RabbitMqClientConfiguration clientConfiguration = new RabbitMqClientConfiguration();
            ConnectionFactory connectionFactory = new ConnectionFactory();
            IExchangeDeclareParameters exchangeDeclareParameters = new ExchangeDeclareParameters();
            IQueueDeclareParameters queueDeclareParameters = new QueueDeclareParameters();
            RabbitMqSinkConfiguration sinkConfiguration = new RabbitMqSinkConfiguration();
            configure(connectionFactory, exchangeDeclareParameters, queueDeclareParameters, sinkConfiguration);

            return RegisterSink(loggerConfiguration, connectionFactory, exchangeDeclareParameters, queueDeclareParameters, sinkConfiguration);
        }

        /// <summary>
        /// Adds a sink that lets you push log messages to RabbitMQ
        /// Will be used when configuring via configuration file
        /// If you need to overrule the text formatter, you will need to supply it here as a separate parameter instead of supplying it via the RabbitMQSinkConfiguration instance
        /// which will not work when configuring via configuration file
        /// </summary>
        public static LoggerConfiguration RabbitMq(
            this LoggerSinkConfiguration loggerConfiguration,
            IConnectionFactory connectionFactory,
            IExchangeDeclareParameters exchangeDeclareParameters,
            IQueueDeclareParameters queueDeclareParameters,
            RabbitMqSinkConfiguration sinkConfiguration, ITextFormatter textFormatter = null)
        {
            if (textFormatter != null) sinkConfiguration.TextFormatter = textFormatter;
            return RegisterSink(loggerConfiguration, connectionFactory, exchangeDeclareParameters, queueDeclareParameters, sinkConfiguration);
        }

        /// <summary>
        /// Adds a sink that lets you push log messages to RabbitMQ
        /// Will be used when configuring via configuration file
        /// Is for backward-compatibility with previous version
        /// </summary>
        public static LoggerConfiguration RabbitMq(
            this LoggerSinkConfiguration loggerConfiguration,
            string mqHostName, string mqUserName, string mqPassword, string brokerName, Base.EventBus.ExchangeType exchangeType,
            bool exchangeDurable = false, bool exchangeAutoDelete = false,
            bool queueDurable = false, bool queueExclusive = false, bool queueAutoDelete = false,
            int batchPostingLimit = 0,
            TimeSpan period = default, ITextFormatter formatter = null, IFormatProvider formatProvider = null
        )
        {
            return loggerConfiguration.RabbitMq(mqHostName, mqUserName, mqPassword, 
                brokerName, exchangeType, exchangeDurable, exchangeAutoDelete, 
                queueDurable, queueExclusive, queueAutoDelete, 
                batchPostingLimit, period, formatter);
        }

        /// <summary>
        /// Adds a sink that lets you push log messages to RabbitMQ
        /// Will be used when configuring via configuration file
        /// Is for backward-compatibility with previous version but gives possibility to use multiple hosts
        /// </summary>
        public static LoggerConfiguration RabbitMq(
            this LoggerSinkConfiguration loggerConfiguration,
            string mqHostName, string mqUserName, string mqPassword, string brokerName, Base.EventBus.ExchangeType exchangeType,
            bool exchangeDurable = false, bool exchangeAutoDelete = false,
            bool queueDurable = false, bool queueExclusive = false, bool queueAutoDelete = false, 
            int batchPostingLimit = 0, TimeSpan period = default, ITextFormatter textFormatter = null
        )
        {

            //RabbitMqClientConfiguration clientConfiguration = new RabbitMqClientConfiguration
            //{
            //    Username = username,
            //    Password = password,
            //    Exchange = exchange,
            //    ExchangeType = exchangeType,
            //    DeliveryMode = deliveryMode,
            //    RouteKey = routeKey,
            //    Port = port,
            //    VHost = vHost,
            //    Heartbeat = heartbeat,
            //    Protocol = protocol
            //};
            //foreach (string hostname in hostnames)
            //{
            //    clientConfiguration.Hostnames.Add(hostname);
            //}

            IConnectionFactory connectionFactory = new ConnectionFactory
            {
                HostName = mqHostName,
                UserName = mqUserName,
                Password = mqPassword,
                AutomaticRecoveryEnabled = true,
                NetworkRecoveryInterval = TimeSpan.FromSeconds(10),
                RequestedHeartbeat = TimeSpan.FromSeconds(10),
                DispatchConsumersAsync = true
            };

            IExchangeDeclareParameters exchangeDeclareParameters = new ExchangeDeclareParameters(brokerName, exchangeType, exchangeDurable, exchangeAutoDelete);
            IQueueDeclareParameters queueDeclareParameters = new QueueDeclareParameters(queueDurable, queueExclusive, queueAutoDelete);

            RabbitMqSinkConfiguration sinkConfiguration = new RabbitMqSinkConfiguration
            {
                BatchPostingLimit = batchPostingLimit,
                Period = period,
                TextFormatter = textFormatter
            };

            return RegisterSink(loggerConfiguration, connectionFactory, exchangeDeclareParameters, queueDeclareParameters, sinkConfiguration);
        }

        static LoggerConfiguration RegisterSink(
            LoggerSinkConfiguration loggerConfiguration, 
            IConnectionFactory connectionFactory,
            IExchangeDeclareParameters exchangeDeclareParameters,
            IQueueDeclareParameters queueDeclareParameters, 
            RabbitMqSinkConfiguration sinkConfiguration)
        {
            // guards
            if (loggerConfiguration == null) throw new ArgumentNullException(nameof(loggerConfiguration));
            //if (connectionFactory.HostN == 0) throw new ArgumentException("hostnames cannot be empty, specify at least one hostname", "hostnames");
            if (string.IsNullOrEmpty(connectionFactory.UserName)) throw new ArgumentException("username cannot be 'null' or and empty string.");
            if (connectionFactory.Password == null) throw new ArgumentException("password cannot be 'null'. Specify an empty string if password is empty.");
            //if (connectionFactory.Port <= 0 || clientConfiguration.Port > 65535) throw new ArgumentOutOfRangeException("port", "port must be in a valid range (1 and 65535)");

            sinkConfiguration.BatchPostingLimit = (sinkConfiguration.BatchPostingLimit == default(int)) ? DefaultBatchPostingLimit : sinkConfiguration.BatchPostingLimit;
            sinkConfiguration.Period = (sinkConfiguration.Period == default) ? DefaultPeriod : sinkConfiguration.Period;

            return
                loggerConfiguration
                    .Sink(new RabbitMqSink(connectionFactory, exchangeDeclareParameters, queueDeclareParameters, sinkConfiguration), sinkConfiguration.RestrictedToMinimumLevel);
        }
    }
}
