namespace KSociety.Log.Serilog.Sinks.RichTextBox.Wpf.Shared.Sinks.RichTextBox.Formatting;
using System.IO;

internal struct ThemedValueFormatterState
{
    public TextWriter Output;
    public string Format;
    public bool IsTopLevel;

    public ThemedValueFormatterState Nest()
    {
        return new ThemedValueFormatterState {Output = this.Output};
    }
}
