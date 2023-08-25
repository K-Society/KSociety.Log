namespace KSociety.Log.Serilog.Sinks.RabbitMq.Sinks.RabbitMq
{
    using global::Serilog.Sinks.PeriodicBatching;

    public interface IRabbitMqBatchedSink : IBatchedLogEventSink
    {
        void Initialize();
    }
}