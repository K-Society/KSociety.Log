using System;
using Serilog.Events;
using Serilog.Formatting;
using Serilog.Formatting.Compact;

namespace KSociety.Log.Serilog.Sinks.RabbitMq.Sinks.RabbitMq;

public class RabbitMqSinkConfiguration
{
    public int BatchPostingLimit { get; set; }
    public TimeSpan Period { get; set; }
    public ITextFormatter TextFormatter { get; set; } = new CompactJsonFormatter();
    public LogEventLevel RestrictedToMinimumLevel { get; set; } = LogEventLevel.Verbose;
}