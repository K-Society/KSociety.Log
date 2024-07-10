// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

//namespace Std.ComDriver.Test
//{
//    /// <summary>
//    ///   Tests for <see cref="RabbitMqClient" />.
//    /// </summary>
//    [Collection("Sequential")]
//    public sealed class RabbitMqClientTest : IDisposable
//    {
//        private const string QueueName = "serilog-sink-queue";
//        private const string HostName = "localhost";

//        private readonly IConnectionFactory _connectionFactory;
//        private readonly IExchangeDeclareParameters _exchangeDeclareParameters;
//        private readonly IQueueDeclareParameters _queueDeclareParameters;

//        private ILoggerFactory _loggerFactory { get; }
//        private IRabbitMqPersistentConnection _persistentConnection { get; set; }
//        private IEventBus _eventBus;

//        //private readonly RabbitMqClient client = new RabbitMqClient(new RabbitMqClientConfiguration
//        //{
//        //    Port = 5672,
//        //    DeliveryMode = RabbitMqDeliveryMode.NonDurable,
//        //    Exchange = "k-society_log_test",
//        //    Username = "KSociety",
//        //    Password = "KSociety",
//        //    ExchangeType = "fanout",
//        //    Hostnames = { HostName }
//        //});

//        private IConnection connection;
//        private IModel channel;

//        private void Initialize()
//        {
//            _persistentConnection = new DefaultRabbitMqPersistentConnection(_connectionFactory, _loggerFactory);

//            _eventBus = new EventBusRabbitMqTyped(
//                _persistentConnection,
//                _loggerFactory,
//                null,
//                _exchangeDeclareParameters, _queueDeclareParameters,
//                "LogQueueDriver", CancellationToken.None);
//        }

//        /// <summary>
//        ///   Consumer should receive a message after calling Publish.
//        /// </summary>
//        /// <returns>A task that represents the asynchronous operation.</returns>.
//        [Fact]
//        public async Task Publish_SingleMessage_ConsumerReceivesMessage()
//        {
//            await InitializeAsync();
//            var message = Guid.NewGuid().ToString();

//            var consumer = new EventingBasicConsumer(channel);
//            var eventRaised = await Assert.RaisesAsync<BasicDeliverEventArgs>(
//                h => consumer.Received += h,
//                h => consumer.Received -= h,
//                async () =>
//                {
//                    channel.BasicConsume(QueueName, autoAck: true, consumer);
//                    await client.PublishAsync(message);

//                    // Wait for consumer to receive the message.
//                    await Task.Delay(50);
//                });

//            var receivedMessage = Encoding.UTF8.GetString(eventRaised.Arguments.Body.ToArray());
//            Assert.Equal(message, receivedMessage);
//        }

//        /// <inheritdoc />
//        public void Dispose()
//        {
//            client?.Dispose();
//            channel?.Dispose();
//            connection?.Dispose();
//        }

//        private async Task InitializeAsync()
//        {
//            if (connection == null)
//            {
//                var factory = new ConnectionFactory { HostName = HostName };

//                // Wait for RabbitMQ docker container to start and retry connecting to it.
//                for (int i = 0; i < 10; ++i)
//                {
//                    try
//                    {
//                        connection = factory.CreateConnection();
//                        channel = connection.CreateModel();
//                        break;
//                    }
//                    catch (BrokerUnreachableException)
//                    {
//                        await Task.Delay(1000);
//                    }
//                }
//            }
//        }
//    }
//}
