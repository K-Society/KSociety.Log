namespace KSociety.Log.Serilog.Sinks.RichTextBox.Wpf.Shared.Sinks.RichTextBox.Abstraction;

using System;
using System.Threading.Tasks;
using global::Serilog.Events;

public interface IRichTextBox : IObserver<LogEvent>
{
    void StartRichTextBoxLimiter();

    void StopRichTextBoxLimiter();

    bool CheckAccess();

    ValueTask BeginInvoke(string? xamlParagraphText);
}
