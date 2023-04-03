using System.IO;

namespace KSociety.Log.Serilog.Sinks.RichTextBox.Wpf.Sinks.RichTextBox.Formatting
{
    internal struct ThemedValueFormatterState
    {
        public TextWriter Output;
        public string Format;
        public bool IsTopLevel;

        public ThemedValueFormatterState Nest() => new ThemedValueFormatterState { Output = Output };
    }
}
