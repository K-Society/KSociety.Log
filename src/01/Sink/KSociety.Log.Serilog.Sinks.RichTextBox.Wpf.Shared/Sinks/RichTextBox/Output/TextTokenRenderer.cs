using System.IO;
using KSociety.Log.Serilog.Sinks.RichTextBox.Wpf.Shared.Sinks.RichTextBox.Rendering;
using KSociety.Log.Serilog.Sinks.RichTextBox.Wpf.Shared.Sinks.RichTextBox.Themes;
using Serilog.Events;

namespace KSociety.Log.Serilog.Sinks.RichTextBox.Wpf.Shared.Sinks.RichTextBox.Output;

internal class TextTokenRenderer : OutputTemplateTokenRenderer
{
    private readonly RichTextBoxTheme _theme;
    private readonly string _text;

    public TextTokenRenderer(RichTextBoxTheme theme, string text)
    {
        _theme = theme;
        _text = text;
    }

    public override void Render(LogEvent logEvent, TextWriter output)
    {
        var _ = 0;
        var text = _text;

        using (_theme.Apply(output, RichTextBoxThemeStyle.TertiaryText, ref _))
        {
            text = SpecialCharsEscaping.Apply(text, ref _);
            output.Write(text);
        }
    }
}