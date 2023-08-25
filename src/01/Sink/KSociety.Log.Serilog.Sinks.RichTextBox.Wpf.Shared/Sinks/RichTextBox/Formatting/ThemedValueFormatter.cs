namespace KSociety.Log.Serilog.Sinks.RichTextBox.Wpf.Shared.Sinks.RichTextBox.Formatting;
using System;
using System.IO;
using KSociety.Log.Serilog.Sinks.RichTextBox.Wpf.Shared.Sinks.RichTextBox.Themes;
using global::Serilog.Data;
using global::Serilog.Events;

internal abstract class ThemedValueFormatter : LogEventPropertyValueVisitor<ThemedValueFormatterState, int>
{
    private readonly RichTextBoxTheme _theme;

    protected ThemedValueFormatter(RichTextBoxTheme theme)
    {
        this._theme = theme ?? throw new ArgumentNullException(nameof(theme));
    }

    protected StyleReset ApplyStyle(TextWriter output, RichTextBoxThemeStyle style, ref int invisibleCharacterCount)
    {
        return this._theme.Apply(output, style, ref invisibleCharacterCount);
    }

    public int Format(LogEventPropertyValue value, TextWriter output, string format, bool literalTopLevel = false)
    {
        return this.Visit(new ThemedValueFormatterState { Output = output, Format = format, IsTopLevel = literalTopLevel }, value);
    }

    public abstract ThemedValueFormatter SwitchTheme(RichTextBoxTheme theme);
}