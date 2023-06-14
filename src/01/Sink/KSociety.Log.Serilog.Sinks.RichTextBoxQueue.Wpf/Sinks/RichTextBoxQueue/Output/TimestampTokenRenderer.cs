using System;
using System.IO;
using KSociety.Log.Serilog.Sinks.RichTextBoxQueue.Wpf.Sinks.RichTextBoxQueue.Rendering;
using KSociety.Log.Serilog.Sinks.RichTextBoxQueue.Wpf.Sinks.RichTextBoxQueue.Themes;
using Serilog.Events;
using Serilog.Parsing;

namespace KSociety.Log.Serilog.Sinks.RichTextBoxQueue.Wpf.Sinks.RichTextBoxQueue.Output
{
    internal class TimestampTokenRenderer : OutputTemplateTokenRenderer
    {
        private readonly RichTextBoxTheme _theme;
        private readonly PropertyToken _token;
        private readonly IFormatProvider _formatProvider;

        public TimestampTokenRenderer(RichTextBoxTheme theme, PropertyToken token, IFormatProvider formatProvider)
        {
            _theme = theme;
            _token = token;
            _formatProvider = formatProvider;
        }

        public override void Render(LogEvent logEvent, TextWriter output)
        {
            // We need access to ScalarValue.Render() to avoid this alloc; just ensures
            // that custom format providers are supported properly.
            var sv = new ScalarValue(logEvent.Timestamp);

            var _ = 0;

            using (_theme.Apply(output, RichTextBoxThemeStyle.SecondaryText, ref _))
            {
                if (_token.Alignment is null)
                {
                    sv.Render(output, _token.Format, _formatProvider);
                }
                else
                {
                    var buffer = new StringWriter();
                    sv.Render(buffer, _token.Format, _formatProvider);

                    var str = buffer.ToString();
                    Padding.Apply(output, str, _token.Alignment);
                }
            }
        }
    }
}
