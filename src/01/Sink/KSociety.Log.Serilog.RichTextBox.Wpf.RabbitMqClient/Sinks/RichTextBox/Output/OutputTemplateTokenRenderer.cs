using System.IO;
using Serilog.Events;

namespace KSociety.Log.Serilog.RichTextBox.Wpf.RabbitMqClient.Sinks.RichTextBox.Output
{
    internal abstract class OutputTemplateTokenRenderer
    {
        public abstract void Render(LogEvent logEvent, TextWriter output);
    }
}
