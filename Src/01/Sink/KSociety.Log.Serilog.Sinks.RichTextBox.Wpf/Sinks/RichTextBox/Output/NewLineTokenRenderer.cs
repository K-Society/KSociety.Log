using KSociety.Log.Serilog.Sinks.RichTextBox.Wpf.Sinks.RichTextBox.Rendering;
using Serilog.Events;
using Serilog.Parsing;
using System;
using System.IO;

namespace KSociety.Log.Serilog.Sinks.RichTextBox.Wpf.Sinks.RichTextBox.Output
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
