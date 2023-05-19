﻿using System;
using System.Windows.Threading;

namespace KSociety.Log.Serilog.RichTextBox.Wpf.RabbitMqClient.Sinks.RichTextBox.Abstraction
{
    internal interface IRichTextBox
    {
        void Write(string xamlParagraphText);

        bool CheckAccess();

        public DispatcherOperation BeginInvoke(DispatcherPriority priority, Delegate method, object arg);
    }
}
