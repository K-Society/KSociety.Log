using System;
using System.IO;
using KSociety.Log.Serilog.Sinks.RichTextBox.Wpf.Shared.Sinks.RichTextBox.Formatting;
using KSociety.Log.Serilog.Sinks.RichTextBox.Wpf.Shared.Sinks.RichTextBox.Rendering;
using KSociety.Log.Serilog.Sinks.RichTextBox.Wpf.Shared.Sinks.RichTextBox.Themes;
using Serilog.Events;
using Serilog.Parsing;

namespace KSociety.Log.Serilog.Sinks.RichTextBox.Wpf.Shared.Sinks.RichTextBox.Output
{
    internal class MessageTemplateOutputTokenRenderer : OutputTemplateTokenRenderer
    {
        private readonly RichTextBoxTheme _theme;
        private readonly PropertyToken _token;
        private readonly ThemedMessageTemplateRenderer _renderer;

        public MessageTemplateOutputTokenRenderer(RichTextBoxTheme theme, PropertyToken token, IFormatProvider formatProvider)
        {
            _theme = theme ?? throw new ArgumentNullException(nameof(theme));
            _token = token ?? throw new ArgumentNullException(nameof(token));

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

            _renderer = new ThemedMessageTemplateRenderer(theme, valueFormatter, isLiteral);
        }

        public override void Render(LogEvent logEvent, TextWriter output)
        {
            if (_token.Alignment is null || !_theme.CanBuffer)
            {
                _renderer.Render(logEvent.MessageTemplate, logEvent.Properties, output);
                return;
            }

            var buffer = new StringWriter();
            var invisible = _renderer.Render(logEvent.MessageTemplate, logEvent.Properties, buffer);
            var value = buffer.ToString();

            Padding.Apply(output, value, _token.Alignment.Value.Widen(invisible));
        }
    }
}
