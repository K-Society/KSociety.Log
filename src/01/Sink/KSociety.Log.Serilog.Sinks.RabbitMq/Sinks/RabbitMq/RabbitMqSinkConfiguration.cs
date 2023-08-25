namespace KSociety.Log.Serilog.Sinks.RabbitMq.Sinks.RabbitMq
{
    using System;
    using global::Serilog.Events;
    using global::Serilog.Formatting;
    using global::Serilog.Formatting.Compact;

    public class RabbitMqSinkConfiguration
    {
        public int BatchPostingLimit { get; set; }
        public TimeSpan Period { get; set; }
        public ITextFormatter TextFormatter { get; set; } = new CompactJsonFormatter();
        public LogEventLevel RestrictedToMinimumLevel { get; set; } = LogEventLevel.Verbose;
    }
}