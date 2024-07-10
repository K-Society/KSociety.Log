// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace KSociety.Log.Serilog.Sinks.RichTextBox.Wpf.Shared.Sinks.RichTextBox.Output
{
    using System;
    using System.IO;
    using Rendering;
    using global::Serilog.Events;
    using global::Serilog.Parsing;

    internal class NewLineTokenRenderer : OutputTemplateTokenRenderer
    {
        private readonly Alignment? _alignment;

        public NewLineTokenRenderer(Alignment? alignment)
        {
            this._alignment = alignment;
        }

        public override void Render(LogEvent logEvent, TextWriter output)
        {
            if (this._alignment.HasValue)
            {
                Padding.Apply(output, Environment.NewLine, this._alignment.Value.Widen(Environment.NewLine.Length));
            }
            else
            {

                //output.WriteLine();
                output.Write(String.Empty);
            }
        }
    }
}
