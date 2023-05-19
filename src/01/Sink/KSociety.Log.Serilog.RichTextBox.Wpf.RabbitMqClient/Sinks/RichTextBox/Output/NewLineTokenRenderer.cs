using System;
using System.IO;
using KSociety.Log.Serilog.RichTextBox.Wpf.RabbitMqClient.Sinks.RichTextBox.Rendering;
using Serilog.Events;
using Serilog.Parsing;

namespace KSociety.Log.Serilog.RichTextBox.Wpf.RabbitMqClient.Sinks.RichTextBox.Output
{
    internal class NewLineTokenRenderer : OutputTemplateTokenRenderer
    {
        private readonly Alignment? _alignment;

        public NewLineTokenRenderer(Alignment? alignment)
        {
            _alignment = alignment;
        }

        public override void Render(LogEvent logEvent, TextWriter output)
        {
            if (_alignment.HasValue)
            {
                Padding.Apply(output, Environment.NewLine, _alignment.Value.Widen(Environment.NewLine.Length));
            }
            else
            {
                output.WriteLine();
            }
        }
    }
}
