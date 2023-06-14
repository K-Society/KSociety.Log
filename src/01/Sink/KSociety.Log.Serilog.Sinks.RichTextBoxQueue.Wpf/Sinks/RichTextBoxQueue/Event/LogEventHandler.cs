using Serilog.Events;
using System;

namespace KSociety.Log.Serilog.Sinks.RichTextBoxQueue.Wpf.Sinks.RichTextBoxQueue.Event
{
    public class LogEventHandler : IObserver<LogEvent>
    {
        public void OnCompleted()
        {
            Console.WriteLine("OnCompleted");
            //throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            Console.WriteLine("OnError");
            //throw new NotImplementedException();
        }

        public void OnNext(LogEvent value)
        {
            Console.WriteLine("OnNext");
            //throw new NotImplementedException();
        }
    }
}
