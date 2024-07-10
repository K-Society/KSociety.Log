// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace KSociety.Log.Serilog.Sinks.RichTextBox.Wpf.Shared.Sinks.RichTextBox.Abstraction
{
    using System;
    using System.Threading.Tasks;
    using global::Serilog.Events;

    public interface IRichTextBox : IObserver<LogEvent>
    {
        void StartRichTextBoxLimiter();

        void StopRichTextBoxLimiter();

        bool CheckAccess();

        ValueTask BeginInvoke(string? xamlParagraphText);
    }
}
