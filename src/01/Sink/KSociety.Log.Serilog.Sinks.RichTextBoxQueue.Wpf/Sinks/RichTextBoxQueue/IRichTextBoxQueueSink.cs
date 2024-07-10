// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace KSociety.Log.Serilog.Sinks.RichTextBoxQueue.Wpf.Sinks.RichTextBoxQueue
{
    using System;
    using System.Windows.Controls;
    using System.Windows.Threading;
    using KSociety.Log.Serilog.Sinks.RichTextBox.Wpf.Shared.Sinks.RichTextBox.Themes;

    public interface IRichTextBoxQueueSink
    {
        void AddRichTextBox(RichTextBox? richTextBoxControl,
            DispatcherPriority dispatcherPriority = DispatcherPriority.Background, IFormatProvider? formatProvider = null,
            RichTextBoxTheme? theme = null, object? syncRoot = null);
    }
}
