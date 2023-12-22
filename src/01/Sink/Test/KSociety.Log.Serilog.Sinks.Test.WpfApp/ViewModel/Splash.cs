// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace KSociety.Log.Serilog.Sinks.Test.WpfApp.ViewModel
{
    using System.Windows.Input;
    using CommunityToolkit.Mvvm.ComponentModel;
    using CommunityToolkit.Mvvm.Input;

    public class Splash : ObservableObject
    {
        #region [Fields]

        private double _height;
        private double _width;

        private ICommand _onCompletedCommand;

        #endregion

        #region [Public Properties]

        public double Height
        {
            get { return this._height; }
            set
            {
                if (value != this._height)
                {
                    this._height = value;
                    this.OnPropertyChanged("Height");
                }
            }
        }

        public double Width
        {
            get { return this._width; }
            set
            {
                if (value != this._width)
                {
                    this._width = value;
                    this.OnPropertyChanged("Width");
                }
            }
        }

        public ICommand OnCompletedCommand
        {
            get
            {
                if (this._onCompletedCommand == null)
                {
                    this._onCompletedCommand = new RelayCommand(this.OnCompletedCommandEvent);
                }
                return this._onCompletedCommand;
            }
        }

        #endregion

        public Splash()
        {
            this.Initialize();
        }

        private void Initialize()
        {
            var screen = (System.Windows.SystemParameters.WorkArea.Height / 4) * 3;
            this.Height = screen;
            this.Width = screen;

        }

        private void OnCompletedCommandEvent()
        {
            //if (param is not null)
            //{
            //    if (param is Window window)
            //    {
            //        window.Close();
            //    }
            //}

            //var instance = _serviceProvider.GetRequiredService<TWindow>();//ActivatorUtilities.CreateInstance<LoRaModemWin>(_serviceProvider, node);

            //instance.Show();
        }

        public void AddMessage(string message)
        {
            ;
            //throw new System.NotImplementedException();

            //Dispatcher.Invoke((Action)delegate ()
            //{
            //    //this.UpdateMessageTextBox.Text = message;
            //});
        }

        public void LoadComplete()
        {
            ;
            //throw new System.NotImplementedException();

            //Dispatcher.InvokeShutdown();
        }
    }
}
