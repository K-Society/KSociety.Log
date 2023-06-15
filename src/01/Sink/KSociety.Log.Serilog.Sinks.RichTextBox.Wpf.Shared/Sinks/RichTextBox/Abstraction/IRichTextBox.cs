using System;
using System.Windows.Threading;

namespace KSociety.Log.Serilog.Sinks.RichTextBox.Wpf.Shared.Sinks.RichTextBox.Abstraction
{
    public interface IRichTextBox
    {
        void Write(string xamlParagraphText);

        bool CheckAccess();

        public DispatcherOperation BeginInvoke(DispatcherPriority priority, Delegate method, object arg);
    }
}
