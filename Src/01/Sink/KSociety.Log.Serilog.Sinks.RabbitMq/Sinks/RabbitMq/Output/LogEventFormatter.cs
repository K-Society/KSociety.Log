using Serilog.Events;
using Serilog.Formatting;
using System.IO;

namespace KSociety.Log.Serilog.Sinks.RabbitMq.Sinks.RabbitMq.Output
{
    public class LogEventFormatter : ITextFormatter
    {
        public static LogEventFormatter Formatter { get; } = new LogEventFormatter();

        public void Format(LogEvent logEvent, TextWriter output)
        {
            //output.Write("Timestamp - {0} | Level - {1} | Message {2} {3}", logEvent.Timestamp, logEvent.Level, logEvent.MessageTemplate, output.NewLine);
            output.Write("{0} {1}", logEvent.RenderMessage(), output.NewLine);
            if (logEvent.Exception != null)
            {
                output.Write("Exception - {0}", logEvent.Exception);
            }
        }
    }
}
