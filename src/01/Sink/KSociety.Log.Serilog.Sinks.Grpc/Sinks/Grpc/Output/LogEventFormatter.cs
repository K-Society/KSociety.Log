// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace KSociety.Log.Serilog.Sinks.Grpc.Sinks.Grpc.Output
{
    using global::Serilog.Events;
    using global::Serilog.Formatting;
    using System.IO;

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
