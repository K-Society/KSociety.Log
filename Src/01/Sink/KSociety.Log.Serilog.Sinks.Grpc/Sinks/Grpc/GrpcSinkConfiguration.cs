using Serilog.Events;
using Serilog.Formatting;
using Serilog.Formatting.Compact;
using System;

namespace KSociety.Log.Serilog.Sinks.Grpc.Sinks.Grpc
{
    public class GrpcSinkConfiguration
    {
        public int BatchPostingLimit { get; set; }
        public TimeSpan Period { get; set; }
        public ITextFormatter TextFormatter { get; set; } = new CompactJsonFormatter();
        public LogEventLevel RestrictedToMinimumLevel { get; set; } = LogEventLevel.Verbose;
    }
}
