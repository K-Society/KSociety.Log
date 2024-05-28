// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace KSociety.Log.Serilog.Sinks.RichTextBox.Wpf.Shared.Sinks.RichTextBox.Output
{
    using System.IO;
    using global::Serilog.Events;

    internal abstract class OutputTemplateTokenRenderer
    {
        public abstract void Render(LogEvent logEvent, TextWriter output);
    }
}