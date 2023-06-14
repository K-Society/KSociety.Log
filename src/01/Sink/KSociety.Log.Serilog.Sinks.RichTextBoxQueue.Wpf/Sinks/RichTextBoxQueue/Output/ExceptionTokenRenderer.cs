using System.IO;
using KSociety.Log.Serilog.Sinks.RichTextBoxQueue.Wpf.Sinks.RichTextBoxQueue.Rendering;
using KSociety.Log.Serilog.Sinks.RichTextBoxQueue.Wpf.Sinks.RichTextBoxQueue.Themes;
using Serilog.Events;

namespace KSociety.Log.Serilog.Sinks.RichTextBoxQueue.Wpf.Sinks.RichTextBoxQueue.Output
{
    internal class ExceptionTokenRenderer : OutputTemplateTokenRenderer
    {
        private const string _stackFrameLinePrefix = "   ";

        private readonly RichTextBoxTheme _theme;

        public ExceptionTokenRenderer(RichTextBoxTheme theme)
        {
            _theme = theme;
        }

        public override void Render(LogEvent logEvent, TextWriter output)
        {
            // Padding is never applied by this renderer.

            if (logEvent.Exception is null)
            {
                return;
            }

            var lines = new StringReader(logEvent.Exception.ToString());

            string nextLine;
            while ((nextLine = lines.ReadLine()) != null)
            {
                var style = nextLine.StartsWith(_stackFrameLinePrefix) ? RichTextBoxThemeStyle.SecondaryText : RichTextBoxThemeStyle.Text;
                var _ = 0;

                using (_theme.Apply(output, style, ref _))
                {
                    output.WriteLine(SpecialCharsEscaping.Apply(nextLine, ref _));
                }
            }
        }
    }
}
