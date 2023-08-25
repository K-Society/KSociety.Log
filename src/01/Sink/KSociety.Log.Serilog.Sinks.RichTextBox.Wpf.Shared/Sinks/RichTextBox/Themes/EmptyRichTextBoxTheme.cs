namespace KSociety.Log.Serilog.Sinks.RichTextBox.Wpf.Shared.Sinks.RichTextBox.Themes;
using System.IO;

internal class EmptyRichTextBoxTheme : RichTextBoxTheme
{
    public override bool CanBuffer => true;

    protected override int ResetCharCount { get; } = 0;

    public override int Set(TextWriter output, RichTextBoxThemeStyle style)
    {
        return 0;
    }

    public override void Reset(TextWriter output)
    {
    }
}
