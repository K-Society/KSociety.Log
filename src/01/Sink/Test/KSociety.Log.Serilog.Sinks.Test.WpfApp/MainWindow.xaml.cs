// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace KSociety.Log.Serilog.Sinks.Test.WpfApp
{
    using System;
    using System.ComponentModel;
    using System.Windows;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly ILogger<MainWindow> _logger;
        private readonly IServiceProvider _serviceProvider;

        public MainWindow(ILoggerFactory loggerFactory, IServiceProvider serviceProvider)
        {
            this.InitializeComponent();
            this._loggerFactory = loggerFactory;
            this._logger = loggerFactory.CreateLogger<MainWindow>();
            this._serviceProvider = serviceProvider;
        }

        private void OnExitClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void MainWindow_OnClosing(object? sender, CancelEventArgs e)
        {
            foreach (var window in Application.Current.Windows)
            {
                if (window is Window appWindow)
                {
                    if (appWindow.Equals(Application.Current.MainWindow))
                    {
                        continue;
                    }

                    appWindow.Close();
                }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {


                for (var i = 0; i < 10000; i++)
                {
                    this._logger.LogInformation("Log Message: {0}", i);
                }
            }
            catch (Exception)
            {
                ;
            }
        }
    }
}
