// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace KSociety.Log.Serilog.Sinks.RichTextBox.Wpf.Shared.Sinks.RichTextBox.Formatting
{
    using System.IO;

    internal struct ThemedValueFormatterState
    {
        public TextWriter Output;
        public string Format;
        public bool IsTopLevel;

        public ThemedValueFormatterState Nest()
        {
            return new ThemedValueFormatterState {Output = this.Output};
        }
    }
}
