namespace KSociety.Log.Serilog.Sinks.RichTextBox.Wpf.Shared.Sinks.RichTextBox.Formatting;
using System;
using System.Globalization;
using System.IO;
using Rendering;
using Themes;
using global::Serilog.Events;
using global::Serilog.Formatting.Json;

internal class ThemedDisplayValueFormatter : ThemedValueFormatter
{
    private readonly IFormatProvider _formatProvider;

    public ThemedDisplayValueFormatter(RichTextBoxTheme theme, IFormatProvider formatProvider)
        : base(theme)
    {
        this._formatProvider = formatProvider;
    }

    public override ThemedValueFormatter SwitchTheme(RichTextBoxTheme theme)
    {
        return new ThemedDisplayValueFormatter(theme, this._formatProvider);
    }

    protected override int VisitScalarValue(ThemedValueFormatterState state, ScalarValue scalar)
    {
        if (scalar is null)
        {
            throw new ArgumentNullException(nameof(scalar));
        }

        return this.FormatLiteralValue(scalar, state.Output, state.Format);
    }

    protected override int VisitSequenceValue(ThemedValueFormatterState state, SequenceValue sequence)
    {
        if (sequence is null)
        {
            throw new ArgumentNullException(nameof(sequence));
        }

        var count = 0;

        using (this.ApplyStyle(state.Output, RichTextBoxThemeStyle.TertiaryText, ref count))
        {
            state.Output.Write('[');
        }

        var delim = String.Empty;

        // ReSharper disable once ForCanBeConvertedToForeach
        for (var index = 0; index < sequence.Elements.Count; ++index)
        {
            if (delim.Length != 0)
            {
                using (this.ApplyStyle(state.Output, RichTextBoxThemeStyle.TertiaryText, ref count))
                {
                    state.Output.Write(delim);
                }
            }

            delim = ", ";
            this.Visit(state, sequence.Elements[index]);
        }

        using (this.ApplyStyle(state.Output, RichTextBoxThemeStyle.TertiaryText, ref count))
        {
            state.Output.Write(']');
        }

        return count;
    }

    protected override int VisitStructureValue(ThemedValueFormatterState state, StructureValue structure)
    {
        var count = 0;

        if (structure.TypeTag != null)
        {
            using (this.ApplyStyle(state.Output, RichTextBoxThemeStyle.Name, ref count))
            {
                state.Output.Write(structure.TypeTag);
            }

            state.Output.Write(' ');
        }

        using (this.ApplyStyle(state.Output, RichTextBoxThemeStyle.TertiaryText, ref count))
        {
            state.Output.Write('{');
        }

        var delim = String.Empty;

        // ReSharper disable once ForCanBeConvertedToForeach
        for (var index = 0; index < structure.Properties.Count; ++index)
        {
            if (delim.Length != 0)
            {
                using (this.ApplyStyle(state.Output, RichTextBoxThemeStyle.TertiaryText, ref count))
                {
                    state.Output.Write(delim);
                }
            }

            delim = ", ";

            var property = structure.Properties[index];

            using (this.ApplyStyle(state.Output, RichTextBoxThemeStyle.Name, ref count))
            {
                var escapedPropertyName = SpecialCharsEscaping.Apply(property.Name, ref count);
                state.Output.Write(escapedPropertyName);
            }

            using (this.ApplyStyle(state.Output, RichTextBoxThemeStyle.TertiaryText, ref count))
            {
                state.Output.Write('=');
            }

            count += this.Visit(state.Nest(), property.Value);
        }

        using (this.ApplyStyle(state.Output, RichTextBoxThemeStyle.TertiaryText, ref count))
        {
            state.Output.Write('}');
        }

        return count;
    }

    protected override int VisitDictionaryValue(ThemedValueFormatterState state, DictionaryValue dictionary)
    {
        var count = 0;

        using (this.ApplyStyle(state.Output, RichTextBoxThemeStyle.TertiaryText, ref count))
        {
            state.Output.Write('{');
        }

        var delim = String.Empty;
        foreach (var element in dictionary.Elements)
        {
            if (delim.Length != 0)
            {
                using (this.ApplyStyle(state.Output, RichTextBoxThemeStyle.TertiaryText, ref count))
                {
                    state.Output.Write(delim);
                }
            }

            delim = ", ";

            using (this.ApplyStyle(state.Output, RichTextBoxThemeStyle.TertiaryText, ref count))
            {
                state.Output.Write('[');
            }

            using (this.ApplyStyle(state.Output, RichTextBoxThemeStyle.String, ref count))
            {
                count += this.Visit(state.Nest(), element.Key);
            }

            using (this.ApplyStyle(state.Output, RichTextBoxThemeStyle.TertiaryText, ref count))
            {
                state.Output.Write("]=");
            }

            count += this.Visit(state.Nest(), element.Value);
        }

        using (this.ApplyStyle(state.Output, RichTextBoxThemeStyle.TertiaryText, ref count))
        {
            state.Output.Write('}');
        }

        return count;
    }

    public int FormatLiteralValue(ScalarValue scalar, TextWriter output, string format)
    {
        var value = scalar.Value;
        var count = 0;

        if (value is null)
        {
            using (this.ApplyStyle(output, RichTextBoxThemeStyle.Null, ref count))
            {
                output.Write("null");
            }

            return count;
        }

        if (value is string str)
        {
            using (this.ApplyStyle(output, RichTextBoxThemeStyle.String, ref count))
            {
                var escapedStr = SpecialCharsEscaping.Apply(str, ref count);

                if (format != "l")
                {
                    JsonValueFormatter.WriteQuotedJsonString(escapedStr, output);
                }
                else
                {
                    output.Write(escapedStr);
                }
            }

            return count;
        }

        if (value is ValueType)
        {
            if (value is int || value is uint || value is long || value is ulong ||
                value is decimal || value is byte || value is sbyte || value is short ||
                value is ushort || value is float || value is double)
            {
                using (this.ApplyStyle(output, RichTextBoxThemeStyle.Number, ref count))
                {
                    scalar.Render(output, format, this._formatProvider);
                }

                return count;
            }

            if (value is bool b)
            {
                using (this.ApplyStyle(output, RichTextBoxThemeStyle.Boolean, ref count))
                {
                    output.Write(b.ToString(CultureInfo.InvariantCulture));
                }

                return count;
            }

            if (value is char ch)
            {
                using (this.ApplyStyle(output, RichTextBoxThemeStyle.Scalar, ref count))
                {
                    output.Write('\'');
                    output.Write(SpecialCharsEscaping.Apply(ch.ToString(), ref count));
                    output.Write('\'');
                }

                return count;
            }
        }

        using (this.ApplyStyle(output, RichTextBoxThemeStyle.Scalar, ref count))
        {
            scalar.Render(output, format, this._formatProvider);
        }

        return count;
    }
}
