using Serilog.Events;
using System.IO;

namespace KSociety.Log.Serilog.Sinks.RichTextBox.Wpf.Sinks.RichTextBox.Output
{
    internal abstract class OutputTemplateTokenRenderer
    {
        public abstract void Render(LogEvent logEvent, TextWriter output);
    }
}
