namespace KSociety.Log.Serilog.Sinks.RichTextBox.Wpf.Shared.Sinks.RichTextBox.Output;
using System;
using System.IO;
using Rendering;
using Themes;
using global::Serilog.Events;
using global::Serilog.Parsing;

internal class EventPropertyTokenRenderer : OutputTemplateTokenRenderer
{
    private readonly RichTextBoxTheme _theme;
    private readonly PropertyToken _token;
    private readonly IFormatProvider _formatProvider;

    public EventPropertyTokenRenderer(RichTextBoxTheme theme, PropertyToken token, IFormatProvider formatProvider)
    {
        this._theme = theme;
        this._token = token;
        this._formatProvider = formatProvider;
    }

    public override void Render(LogEvent logEvent, TextWriter output)
    {
        // If a property is missing, don't render anything (message templates render the raw token here).
        if (!logEvent.Properties.TryGetValue(this._token.PropertyName, out var propertyValue))
        {
            Padding.Apply(output, String.Empty, this._token.Alignment);
            return;
        }

        var _ = 0;
        using (this._theme.Apply(output, RichTextBoxThemeStyle.SecondaryText, ref _))
        {
            var writer = this._token.Alignment.HasValue ? new StringWriter() : output;

            // If the value is a scalar string, support some additional formats: 'u' for uppercase
            // and 'w' for lowercase.
            if (propertyValue is ScalarValue { Value: string literalString })
            {
                var cased = Casing.Format(literalString, this._token.Format);
                writer.Write(cased);
            }
            else
            {
                propertyValue.Render(writer, this._token.Format, this._formatProvider);
            }

            if (this._token.Alignment.HasValue)
            {
                var str = writer.ToString();
                Padding.Apply(output, str, this._token.Alignment);
            }
        }
    }
}
