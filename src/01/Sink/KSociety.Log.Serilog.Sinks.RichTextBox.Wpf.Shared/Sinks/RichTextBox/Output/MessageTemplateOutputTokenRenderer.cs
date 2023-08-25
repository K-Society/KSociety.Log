namespace KSociety.Log.Serilog.Sinks.RichTextBox.Wpf.Shared.Sinks.RichTextBox.Output;
using System;
using System.IO;
using KSociety.Log.Serilog.Sinks.RichTextBox.Wpf.Shared.Sinks.RichTextBox.Formatting;
using KSociety.Log.Serilog.Sinks.RichTextBox.Wpf.Shared.Sinks.RichTextBox.Rendering;
using KSociety.Log.Serilog.Sinks.RichTextBox.Wpf.Shared.Sinks.RichTextBox.Themes;
using global::Serilog.Events;
using global::Serilog.Parsing;

internal class MessageTemplateOutputTokenRenderer : OutputTemplateTokenRenderer
{
    private readonly RichTextBoxTheme _theme;
    private readonly PropertyToken _token;
    private readonly ThemedMessageTemplateRenderer _renderer;

    public MessageTemplateOutputTokenRenderer(RichTextBoxTheme theme, PropertyToken token, IFormatProvider formatProvider)
    {
        this._theme = theme ?? throw new ArgumentNullException(nameof(theme));
        this._token = token ?? throw new ArgumentNullException(nameof(token));

        var isLiteral = false;
        var isJson = false;

        if (token.Format != null)
        {
            // ReSharper disable once ForCanBeConvertedToForeach
            for (var i = 0; i < token.Format.Length; ++i)
            {
                switch (token.Format[i])
                {
                    case 'l':
                        isLiteral = true;
                        break;
                    case 'j':
                        isJson = true;
                        break;
                }
            }
        }

        var valueFormatter = isJson
            ? (ThemedValueFormatter)new ThemedJsonValueFormatter(theme, formatProvider)
            : new ThemedDisplayValueFormatter(theme, formatProvider);

        this._renderer = new ThemedMessageTemplateRenderer(theme, valueFormatter, isLiteral);
    }

    public override void Render(LogEvent logEvent, TextWriter output)
    {
        if (this._token.Alignment is null || !this._theme.CanBuffer)
        {
            this._renderer.Render(logEvent.MessageTemplate, logEvent.Properties, output);
            return;
        }

        var buffer = new StringWriter();
        var invisible = this._renderer.Render(logEvent.MessageTemplate, logEvent.Properties, buffer);
        var value = buffer.ToString();

        Padding.Apply(output, value, this._token.Alignment.Value.Widen(invisible));
    }
}