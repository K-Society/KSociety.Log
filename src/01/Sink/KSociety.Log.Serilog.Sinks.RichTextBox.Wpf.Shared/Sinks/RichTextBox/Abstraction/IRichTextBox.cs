namespace KSociety.Log.Serilog.Sinks.RichTextBox.Wpf.Shared.Sinks.RichTextBox.Abstraction;
using System.Threading.Tasks;
using System.Windows.Threading;

public interface IRichTextBox
{
    void LimitRows();

    //void Write(string xamlParagraphText);

    bool CheckAccess();

    ValueTask BeginInvoke(DispatcherPriority priority, /*Delegate method,*/ string arg);
}
