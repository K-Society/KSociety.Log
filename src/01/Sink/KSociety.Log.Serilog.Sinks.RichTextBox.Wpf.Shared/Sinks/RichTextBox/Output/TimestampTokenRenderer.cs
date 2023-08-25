namespace KSociety.Log.Serilog.Sinks.RichTextBox.Wpf.Shared.Sinks.RichTextBox.Output;
using System;
using System.IO;
using KSociety.Log.Serilog.Sinks.RichTextBox.Wpf.Shared.Sinks.RichTextBox.Rendering;
using KSociety.Log.Serilog.Sinks.RichTextBox.Wpf.Shared.Sinks.RichTextBox.Themes;
using global::Serilog.Events;
using global::Serilog.Parsing;

internal class TimestampTokenRenderer : OutputTemplateTokenRenderer
{
    private readonly RichTextBoxTheme _theme;
    private readonly PropertyToken _token;
    private readonly IFormatProvider _formatProvider;

    public TimestampTokenRenderer(RichTextBoxTheme theme, PropertyToken token, IFormatProvider formatProvider)
    {
        this._theme = theme;
        this._token = token;
        this._formatProvider = formatProvider;
    }

    public override void Render(LogEvent logEvent, TextWriter output)
    {
        // We need access to ScalarValue.Render() to avoid this alloc; just ensures
        // that custom format providers are supported properly.
        var sv = new ScalarValue(logEvent.Timestamp);

        var _ = 0;

        using (this._theme.Apply(output, RichTextBoxThemeStyle.SecondaryText, ref _))
        {
            if (this._token.Alignment is null)
            {
                sv.Render(output, this._token.Format, this._formatProvider);
            }
            else
            {
                var buffer = new StringWriter();
                sv.Render(buffer, this._token.Format, this._formatProvider);

                var str = buffer.ToString();
                Padding.Apply(output, str, this._token.Alignment);
            }
        }
    }
}