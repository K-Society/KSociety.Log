// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace KSociety.Log.Serilog.Sinks.RichTextBox.Wpf.Shared.Sinks.RichTextBox.Output
{
    using System.IO;
    using Rendering;
    using Themes;
    using global::Serilog.Events;

    internal class ExceptionTokenRenderer : OutputTemplateTokenRenderer
    {
        private const string StackFrameLinePrefix = "   ";

        private readonly RichTextBoxTheme _theme;

        public ExceptionTokenRenderer(RichTextBoxTheme theme)
        {
            this._theme = theme;
        }

        public override void Render(LogEvent logEvent, TextWriter output)
        {
            // Padding is never applied by this renderer.

            if (logEvent.Exception is null)
            {
                return;
            }

            var lines = new StringReader(logEvent.Exception.ToString());

            while (lines.ReadLine() is { } nextLine)
            {
                var style = nextLine.StartsWith(StackFrameLinePrefix) ? RichTextBoxThemeStyle.SecondaryText : RichTextBoxThemeStyle.Text;
                var _ = 0;

                using (this._theme.Apply(output, style, ref _))
                {
                    //output.WriteLine(SpecialCharsEscaping.Apply(nextLine, ref _));
                    output.Write(SpecialCharsEscaping.Apply(nextLine, ref _));
                }
            }
        }
    }
}
