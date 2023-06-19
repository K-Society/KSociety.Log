using System;
using System.IO;
using KSociety.Log.Serilog.Sinks.RichTextBox.Wpf.Shared.Sinks.RichTextBox.Themes;
using Serilog.Data;
using Serilog.Events;

namespace KSociety.Log.Serilog.Sinks.RichTextBox.Wpf.Shared.Sinks.RichTextBox.Formatting;

internal abstract class ThemedValueFormatter : LogEventPropertyValueVisitor<ThemedValueFormatterState, int>
{
    private readonly RichTextBoxTheme _theme;

    protected ThemedValueFormatter(RichTextBoxTheme theme)
    {
        _theme = theme ?? throw new ArgumentNullException(nameof(theme));
    }

    protected StyleReset ApplyStyle(TextWriter output, RichTextBoxThemeStyle style, ref int invisibleCharacterCount)
    {
        return _theme.Apply(output, style, ref invisibleCharacterCount);
    }

    public int Format(LogEventPropertyValue value, TextWriter output, string format, bool literalTopLevel = false)
    {
        return Visit(new ThemedValueFormatterState { Output = output, Format = format, IsTopLevel = literalTopLevel }, value);
    }

    public abstract ThemedValueFormatter SwitchTheme(RichTextBoxTheme theme);
}