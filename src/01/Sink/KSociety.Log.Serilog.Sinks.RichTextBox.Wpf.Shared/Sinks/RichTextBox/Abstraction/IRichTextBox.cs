namespace KSociety.Log.Serilog.Sinks.RichTextBox.Wpf.Shared.Sinks.RichTextBox.Abstraction;

using System;
using System.Threading.Tasks;
using System.Windows.Threading;
using global::Serilog.Events;

public interface IRichTextBox : IObserver<LogEvent>
{
    //void Write(string xamlParagraphText);

    bool CheckAccess();

    ValueTask BeginInvoke(DispatcherPriority priority, string? xamlParagraphText);
}
