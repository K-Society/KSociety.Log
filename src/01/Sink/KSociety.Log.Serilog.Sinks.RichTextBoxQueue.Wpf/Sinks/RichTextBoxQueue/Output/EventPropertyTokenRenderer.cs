using System;
using System.IO;
using KSociety.Log.Serilog.Sinks.RichTextBoxQueue.Wpf.Sinks.RichTextBoxQueue.Rendering;
using KSociety.Log.Serilog.Sinks.RichTextBoxQueue.Wpf.Sinks.RichTextBoxQueue.Themes;
using Serilog.Events;
using Serilog.Parsing;

namespace KSociety.Log.Serilog.Sinks.RichTextBoxQueue.Wpf.Sinks.RichTextBoxQueue.Output
{
    internal class EventPropertyTokenRenderer : OutputTemplateTokenRenderer
    {
        private readonly RichTextBoxTheme _theme;
        private readonly PropertyToken _token;
        private readonly IFormatProvider _formatProvider;

        public EventPropertyTokenRenderer(RichTextBoxTheme theme, PropertyToken token, IFormatProvider formatProvider)
        {
            _theme = theme;
            _token = token;
            _formatProvider = formatProvider;
        }

        public override void Render(LogEvent logEvent, TextWriter output)
        {
            // If a property is missing, don't render anything (message templates render the raw token here).
            if (!logEvent.Properties.TryGetValue(_token.PropertyName, out var propertyValue))
            {
                Padding.Apply(output, string.Empty, _token.Alignment);
                return;
            }

            var _ = 0;
            using (_theme.Apply(output, RichTextBoxThemeStyle.SecondaryText, ref _))
            {
                var writer = _token.Alignment.HasValue ? new StringWriter() : output;

                // If the value is a scalar string, support some additional formats: 'u' for uppercase
                // and 'w' for lowercase.
                if (propertyValue is ScalarValue { Value: string literalString })
                {
                    var cased = Casing.Format(literalString, _token.Format);
                    writer.Write(cased);
                }
                else
                {
                    propertyValue.Render(writer, _token.Format, _formatProvider);
                }

                if (_token.Alignment.HasValue)
                {
                    var str = writer.ToString();
                    Padding.Apply(output, str, _token.Alignment);
                }
            }
        }
    }
}
