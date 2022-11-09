using Serilog.Sinks.PeriodicBatching;

namespace KSociety.Log.Serilog.Sinks.RabbitMq.Sinks.RabbitMq;
public interface IRabbitMqBatchedSink : IBatchedLogEventSink
{
    void Initialize();
}
