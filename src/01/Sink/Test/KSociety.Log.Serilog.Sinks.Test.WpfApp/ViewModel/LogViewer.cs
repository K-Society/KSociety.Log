// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace KSociety.Log.Serilog.Sinks.Test.WpfApp.ViewModel
{
    using System;
    using CommunityToolkit.Mvvm.ComponentModel;
    using KSociety.Log.Serilog.Sinks.RichTextBoxQueue.Wpf.Sinks.RichTextBoxQueue;
    using Microsoft.Extensions.Logging;

    public class LogViewer : ObservableObject
    {
        private readonly ILogger<LogViewer>? _logger;
        private readonly IRichTextBoxQueueSink? _richTextBoxQueueSink;

        #region [Fields]

        private readonly System.Windows.Controls.RichTextBox? _richTextBox;

        private readonly object _syncRoot;

        #endregion

        public LogViewer()
        {
            this._syncRoot = new object();
        }

        public LogViewer(IRichTextBoxQueueSink richTextBoxQueueSink, ILogger<LogViewer> logger, System.Windows.Controls.RichTextBox richTextBox)
        :this()
        {
            this._richTextBoxQueueSink = richTextBoxQueueSink;
            this._logger = logger;
            this._richTextBox = richTextBox;

            try
            {
                this._richTextBoxQueueSink.AddRichTextBox(this._richTextBox, syncRoot: this._syncRoot);
            }
            catch (Exception)
            {
                ;
            }
        }
    }
}
