using System;
using Serilog.Events;

namespace KSociety.Log.Serilog.Sinks.RichTextBox.Wpf.Shared.Sinks.RichTextBox.Event
{
    public class LogEventHandler : IObserver<LogEvent>
    {
        public void OnCompleted()
        {
            Console.WriteLine("OnCompleted");
        }

        public void OnError(Exception error)
        {
            Console.WriteLine("OnError {0}", error.Message);
        }

        public void OnNext(LogEvent value)
        {
            Console.WriteLine("OnNext: {0}", value.RenderMessage());
        }
    }
}
