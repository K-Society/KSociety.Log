using System.IO;

namespace KSociety.Log.Serilog.Sinks.RichTextBoxQueue.Wpf.Sinks.RichTextBoxQueue.Formatting
{
    internal struct ThemedValueFormatterState
    {
        public TextWriter Output;
        public string Format;
        public bool IsTopLevel;

        public ThemedValueFormatterState Nest() => new ThemedValueFormatterState { Output = Output };
    }
}
