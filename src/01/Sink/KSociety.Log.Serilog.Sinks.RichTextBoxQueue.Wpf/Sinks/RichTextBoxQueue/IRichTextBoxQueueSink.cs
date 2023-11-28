namespace KSociety.Log.Serilog.Sinks.RichTextBoxQueue.Wpf.Sinks.RichTextBoxQueue;
using System;
using System.Windows.Controls;
using System.Windows.Threading;
using KSociety.Log.Serilog.Sinks.RichTextBox.Wpf.Shared.Sinks.RichTextBox.Themes;

public interface IRichTextBoxQueueSink
{
    void AddRichTextBox(RichTextBox? richTextBoxControl,
        DispatcherPriority dispatcherPriority = DispatcherPriority.Background, IFormatProvider? formatProvider = null,
        RichTextBoxTheme? theme = null, object? syncRoot = null);
}
