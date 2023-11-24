namespace KSociety.Log.Serilog.Sinks.RichTextBox.Wpf.Shared.Sinks.RichTextBox.Abstraction;
using System;
using System.Windows.Threading;

public interface IRichTextBox
{
    void LimitRows();
    void Write(string xamlParagraphText);

    bool CheckAccess();

    public DispatcherOperation BeginInvoke(DispatcherPriority priority, Delegate method, object arg);
}
