namespace KSociety.Log.Serilog.Sinks.RichTextBox.Wpf.Shared.Sinks.RichTextBox.Output;
using System.IO;
using global::Serilog.Events;

internal abstract class OutputTemplateTokenRenderer
{
    public abstract void Render(LogEvent logEvent, TextWriter output);
}