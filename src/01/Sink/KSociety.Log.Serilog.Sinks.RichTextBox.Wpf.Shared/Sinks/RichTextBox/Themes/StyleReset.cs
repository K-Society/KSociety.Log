// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace KSociety.Log.Serilog.Sinks.RichTextBox.Wpf.Shared.Sinks.RichTextBox.Themes
{
    using System;
    using System.IO;

    internal readonly struct StyleReset : IDisposable
    {
        private readonly RichTextBoxTheme _theme;
        private readonly TextWriter _output;

        public StyleReset(RichTextBoxTheme theme, TextWriter output)
        {
            this._theme = theme;
            this._output = output;
        }

        public void Dispose()
        {
            this._theme.Reset(this._output);
        }
    }
}