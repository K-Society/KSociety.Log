using System;
using System.Windows.Threading;

namespace KSociety.Log.Serilog.Sinks.RichTextBoxQueue.Wpf.Sinks.RichTextBoxQueue.Abstraction
{
    internal interface IRichTextBoxQueue
    {
        void Write(string xamlParagraphText);

        bool CheckAccess();

        public DispatcherOperation BeginInvoke(DispatcherPriority priority, Delegate method, object arg);
    }
}
