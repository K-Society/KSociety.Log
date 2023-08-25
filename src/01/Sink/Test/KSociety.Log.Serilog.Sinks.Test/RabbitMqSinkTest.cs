namespace KSociety.Log.Serilog.Sinks.Test;
using System;
using System.Text;
using System.Threading.Tasks;
using KSociety.Log.Serilog.Sinks.RabbitMq;
using KSociety.Log.Serilog.Sinks.RabbitMq.Sinks.RabbitMq;
using Newtonsoft.Json.Linq;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using global::Serilog;
using global::Serilog.Core;
using global::Serilog.Formatting.Json;
using Xunit;

/// <summary>
///   Tests for <see cref="RabbitMqSink" />.
/// </summary>
[Collection("Sequential")]
public sealed class RabbitMqSinkTest : IDisposable
{
    //Std.Serilog.Sinks.RabbitMq.ProtoModel.Configuration.ProtoBufConfiguration();
    private const string QueueName = "LogQueueDriver";
    private const string HostName = "localhost";

    private readonly Logger logger = new LoggerConfiguration()
        .WriteTo.RabbitMq((connectionFactory, exchangeDeclareParameters, queueDeclareParameters, sinkConfiguration) =>
        {
            //clientConfiguration.Port = 5672;
            //clientConfiguration.DeliveryMode = RabbitMqDeliveryMode.NonDurable;
            //clientConfiguration.Exchange = "k-society_log_test";
            //clientConfiguration.Username = "KSociety";
            //clientConfiguration.Password = "KSociety";
            //clientConfiguration.ExchangeType = "fanout";
            //clientConfiguration.Hostnames.Add(HostName);

            //AutomaticRecoveryEnabled = true,
            //NetworkRecoveryInterval = TimeSpan.FromSeconds(10),
            //RequestedHeartbeat = TimeSpan.FromSeconds(10),
            //DispatchConsumersAsync = true

            connectionFactory.HostName = HostName;
            connectionFactory.UserName = "KSociety";
            connectionFactory.Password = "KSociety";
            connectionFactory.AutomaticRecoveryEnabled = true;
            connectionFactory.NetworkRecoveryInterval = TimeSpan.FromSeconds(10);
            connectionFactory.RequestedHeartbeat = TimeSpan.FromSeconds(10);
            connectionFactory.DispatchConsumersAsync = true;

            exchangeDeclareParameters.BrokerName = "k-society_log_test";
            exchangeDeclareParameters.ExchangeType = Base.EventBus.ExchangeType.Direct.ToString().ToLower();
            exchangeDeclareParameters.ExchangeDurable = false;
            exchangeDeclareParameters.ExchangeAutoDelete = true;

            queueDeclareParameters.QueueDurable = false;
            queueDeclareParameters.QueueExclusive = false;
            queueDeclareParameters.QueueAutoDelete = true;


            sinkConfiguration.TextFormatter = new JsonFormatter();
        })
        .MinimumLevel.Verbose()
        .CreateLogger();

    private IConnection connection;
    private IModel channel;

    /// <summary>
    ///   Consumer should receive a message after calling Publish.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>.
    [Fact]
    public async Task Error_LogWithExceptionAndProperties_ConsumerReceivesMessage()
    {
        await this.InitializeAsync();
        var messageTemplate = "Denominator cannot be zero in {numerator}/{denominator}";

        var consumer = new EventingBasicConsumer(this.channel);
        var eventRaised = await Assert.RaisesAsync<BasicDeliverEventArgs>(
            h => consumer.Received += h,
            h => consumer.Received -= h,
            async () =>
            {
                this.channel.BasicConsume(QueueName, autoAck: true, consumer);
                this.logger.Error(new DivideByZeroException(), messageTemplate, 1.0, 0.0);

                // Wait for consumer to receive the message.
                await Task.Delay(50);
            });

        var receivedMessage = JObject.Parse(Encoding.UTF8.GetString(eventRaised.Arguments.Body.ToArray()));
        Assert.Equal("Error", receivedMessage["Level"]);
        Assert.Equal(messageTemplate, receivedMessage["MessageTemplate"]);
        Assert.NotNull(receivedMessage["Properties"]);
        Assert.Equal(1.0, receivedMessage["Properties"]["numerator"]);
        Assert.Equal(0.0, receivedMessage["Properties"]["denominator"]);
        Assert.Equal("System.DivideByZeroException: Attempted to divide by zero.", receivedMessage["Exception"]);
    }

    /// <inheritdoc />
    public void Dispose()
    {
        this.logger?.Dispose();
        this.channel?.Dispose();
        this.connection?.Dispose();
    }

    private async Task InitializeAsync()
    {
        if (this.connection == null)
        {
            var factory = new ConnectionFactory { HostName = HostName };

            // Wait for RabbitMQ docker container to start and retry connecting to it.
            for (int i = 0; i < 10; ++i)
            {
                try
                {
                    this.connection = factory.CreateConnection();
                    this.channel = this.connection.CreateModel();
                    break;
                }
                catch (BrokerUnreachableException)
                {
                    await Task.Delay(1000);
                }
            }
        }
    }
}