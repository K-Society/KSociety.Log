// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace KSociety.Log.Serilog.Sinks.Test.WpfApp
{
    using System;
    using System.Windows;

    /// <summary>
    /// Interaction logic for Splash.xaml
    /// </summary>
    public partial class Splash : Window
    {
        private object? _dataContext;
        private ViewModel.Splash? _splash;

        public Splash()
        {
            this.InitializeComponent();
        }

        private void SplashAnimationOnCompleted(object? sender, EventArgs e)
        {
            this.Close();
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            if (this._dataContext is null)
            {
                this._dataContext = new ViewModel.Splash();
                if (this._dataContext is not null)
                {
                    this._splash = (ViewModel.Splash)this._dataContext;
                    this.DataContext = this._dataContext;
                }
            }
        }
    }
}
