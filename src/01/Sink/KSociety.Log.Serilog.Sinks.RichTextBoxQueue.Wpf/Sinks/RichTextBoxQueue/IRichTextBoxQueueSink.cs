using System.Windows.Threading;
using System;
using KSociety.Log.Serilog.Sinks.RichTextBox.Wpf.Shared.Sinks.RichTextBox.Themes;

namespace KSociety.Log.Serilog.Sinks.RichTextBoxQueue.Wpf.Sinks.RichTextBoxQueue
{
    public interface IRichTextBoxQueueSink
    {
        void AddRichTextBox(System.Windows.Controls.RichTextBox richTextBoxControl,
            DispatcherPriority dispatcherPriority, IFormatProvider? formatProvider = null,
            RichTextBoxTheme? theme = null, object? syncRoot = null);
    }
}
