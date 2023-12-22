// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace KSociety.Log.Serilog.Sinks.Test.WpfApp
{
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for LogViewer.xaml
    /// </summary>
    public partial class LogViewer : UserControl
    {
        private object? _dataContext;
        private ViewModel.LogViewer? _logViewer;

        public LogViewer()
        {
            this.InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (this._dataContext is null)
            {
                this._dataContext = Helper.DiSource.Resolver is null
                    ? new ViewModel.LogViewer()
                    : Helper.DiSource.Resolver(typeof(ViewModel.LogViewer), this.RichTextBox, null);

                if (this._dataContext is not null)
                {
                    this._logViewer = (ViewModel.LogViewer)this._dataContext;
                    this.DataContext = this._dataContext;
                }
            }
        }
    }
}
