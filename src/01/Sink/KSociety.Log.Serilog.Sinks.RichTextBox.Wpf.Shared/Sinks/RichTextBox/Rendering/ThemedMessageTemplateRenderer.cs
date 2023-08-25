namespace KSociety.Log.Serilog.Sinks.RichTextBox.Wpf.Shared.Sinks.RichTextBox.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using KSociety.Log.Serilog.Sinks.RichTextBox.Wpf.Shared.Sinks.RichTextBox.Formatting;
using KSociety.Log.Serilog.Sinks.RichTextBox.Wpf.Shared.Sinks.RichTextBox.Themes;
using global::Serilog.Events;
using global::Serilog.Parsing;

internal class ThemedMessageTemplateRenderer
{
    private readonly RichTextBoxTheme _theme;
    private readonly ThemedValueFormatter _valueFormatter;
    private readonly bool _isLiteral;
    private static readonly RichTextBoxTheme _noTheme = new EmptyRichTextBoxTheme();
    private readonly ThemedValueFormatter _unthemedValueFormatter;

    public ThemedMessageTemplateRenderer(RichTextBoxTheme theme, ThemedValueFormatter valueFormatter, bool isLiteral)
    {
        this._theme = theme ?? throw new ArgumentNullException(nameof(theme));
        this._valueFormatter = valueFormatter;
        this._isLiteral = isLiteral;
        this._unthemedValueFormatter = valueFormatter.SwitchTheme(_noTheme);
    }

    public int Render(MessageTemplate template, IReadOnlyDictionary<string, LogEventPropertyValue> properties, TextWriter output)
    {
        var count = 0;

        foreach (var token in template.Tokens)
        {
            if (token is TextToken tt)
            {
                count += this.RenderTextToken(tt, output);
            }
            else
            {
                var pt = (PropertyToken)token;
                count += this.RenderPropertyToken(pt, properties, output);
            }
        }

        return count;
    }

    private int RenderTextToken(TextToken tt, TextWriter output)
    {
        var count = 0;
        using (this._theme.Apply(output, RichTextBoxThemeStyle.Text, ref count))
        {
            var text = SpecialCharsEscaping.Apply(tt.Text, ref count);
            output.Write(text);
        }

        return count;
    }

    private int RenderPropertyToken(PropertyToken pt, IReadOnlyDictionary<string, LogEventPropertyValue> properties, TextWriter output)
    {
        if (!properties.TryGetValue(pt.PropertyName, out var propertyValue))
        {
            var count = 0;
            using (this._theme.Apply(output, RichTextBoxThemeStyle.Invalid, ref count))
            {
                output.Write(SpecialCharsEscaping.Apply(pt.ToString(), ref count));
            }

            return count;
        }

        if (!pt.Alignment.HasValue)
        {
            return this.RenderValue(this._theme, this._valueFormatter, propertyValue, output, pt.Format);
        }

        var valueOutput = new StringWriter();

        if (!this._theme.CanBuffer)
        {
            return this.RenderAlignedPropertyTokenUnbuffered(pt, output, propertyValue);
        }

        var invisibleCount = this.RenderValue(this._theme, this._valueFormatter, propertyValue, valueOutput, pt.Format);

        var value = valueOutput.ToString();

        if (value.Length - invisibleCount >= pt.Alignment.Value.Width)
        {
            output.Write(value);
        }
        else
        {
            Padding.Apply(output, value, pt.Alignment.Value.Widen(invisibleCount));
        }

        return invisibleCount;
    }

    private int RenderAlignedPropertyTokenUnbuffered(PropertyToken pt, TextWriter output, LogEventPropertyValue propertyValue)
    {
        var valueOutput = new StringWriter();
        this.RenderValue(_noTheme, this._unthemedValueFormatter, propertyValue, valueOutput, pt.Format);

        var valueLength = valueOutput.ToString().Length;

        // ReSharper disable once PossibleInvalidOperationException
        if (valueLength >= pt.Alignment.Value.Width)
        {
            return this.RenderValue(this._theme, this._valueFormatter, propertyValue, output, pt.Format);
        }

        if (pt.Alignment.Value.Direction == AlignmentDirection.Left)
        {
            var invisible = this.RenderValue(this._theme, this._valueFormatter, propertyValue, output, pt.Format);
            Padding.Apply(output, String.Empty, pt.Alignment.Value.Widen(-valueLength));

            return invisible;
        }

        Padding.Apply(output, String.Empty, pt.Alignment.Value.Widen(-valueLength));

        return this.RenderValue(this._theme, this._valueFormatter, propertyValue, output, pt.Format);
    }

    private int RenderValue(RichTextBoxTheme theme, ThemedValueFormatter valueFormatter, LogEventPropertyValue propertyValue, TextWriter output, string format)
    {
        if (this._isLiteral && propertyValue is ScalarValue {Value: string} sv)
        {
            var count = 0;
            using (theme.Apply(output, RichTextBoxThemeStyle.String, ref count))
            {
                var text = SpecialCharsEscaping.Apply(sv.Value.ToString(), ref count);
                output.Write(text);
            }

            return count;
        }

        return valueFormatter.Format(propertyValue, output, format, this._isLiteral);
    }
}