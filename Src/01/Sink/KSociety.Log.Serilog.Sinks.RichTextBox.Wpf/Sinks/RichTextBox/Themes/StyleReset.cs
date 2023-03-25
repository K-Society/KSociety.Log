using System;
using System.IO;

namespace KSociety.Log.Serilog.Sinks.RichTextBox.Wpf.Sinks.RichTextBox.Themes
{
    internal readonly struct StyleReset : IDisposable
    {
        private readonly RichTextBoxTheme _theme;
        private readonly TextWriter _output;

        public StyleReset(RichTextBoxTheme theme, TextWriter output)
        {
            _theme = theme;
            _output = output;
        }

        public void Dispose()
        {
            _theme.Reset(_output);
        }
    }
}
