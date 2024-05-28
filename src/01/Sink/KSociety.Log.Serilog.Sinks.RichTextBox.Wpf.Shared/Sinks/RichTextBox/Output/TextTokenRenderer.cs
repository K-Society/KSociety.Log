// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace KSociety.Log.Serilog.Sinks.RichTextBox.Wpf.Shared.Sinks.RichTextBox.Output
{
    using System.IO;
    using Rendering;
    using Themes;
    using global::Serilog.Events;

    internal class TextTokenRenderer : OutputTemplateTokenRenderer
    {
        private readonly RichTextBoxTheme _theme;
        private readonly string? _text;

        public TextTokenRenderer(RichTextBoxTheme theme, string? text)
        {
            this._theme = theme;
            this._text = text;
        }

        public override void Render(LogEvent logEvent, TextWriter output)
        {
            var _ = 0;
            var text = this._text;

            using (this._theme.Apply(output, RichTextBoxThemeStyle.TertiaryText, ref _))
            {
                text = SpecialCharsEscaping.Apply(text, ref _);
                output.Write(text);
            }
        }
    }
}
