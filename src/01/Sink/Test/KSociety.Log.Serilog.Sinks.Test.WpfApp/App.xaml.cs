// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace KSociety.Log.Serilog.Sinks.Test.WpfApp
{
    using System;
    using System.Windows;
    using Autofac;
    using Autofac.Extensions.DependencyInjection;
    using Base.InfraSub.Shared.Class;
    using global::Serilog;
    using global::Serilog.Core;
    using KSociety.Log.Serilog.Sinks.Test.WpfApp.Helper;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using RichTextBoxQueue.Wpf;
    using RichTextBoxQueue.Wpf.Sinks.RichTextBoxQueue;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static RichTextBoxQueueSink _richTextBoxQueueSink;
        private readonly IHost _host;
        private static IConfigurationRoot? _configuration;
        private IServiceProvider ServiceProvider { get; set; }

        public object? Resolve(Type? type, object? key, string? name)
        {
            if (type is null)
            {
                return null;
            }

            if (type is not null && key is null)
            {
                return this.ServiceProvider.GetService(type);
            }

            if (type is not null && key is not null)
            {
                return ActivatorUtilities.CreateInstance(this.ServiceProvider, type, key);
            }

            return null;
        }

        public App()
        {
            try
            {
                _configuration = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .Build();

                global::Serilog.Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(_configuration).CreateLogger();

                _richTextBoxQueueSink = new RichTextBoxQueueSink();

                var logEventSink = (ILogEventSink)global::Serilog.Log.Logger;

                var loggerConfiguration = new LoggerConfiguration()
                    .WriteTo.Sink(logEventSink)
                    .MinimumLevel.Verbose()
                    .WriteTo.RichTextBoxQueue(_richTextBoxQueueSink);

                global::Serilog.Log.Logger = loggerConfiguration.CreateLogger();

                var builder = CreateHostBuilder();
                this._host = builder.Build();
            }
            catch (Exception)
            {
                ;
            }
        }

        private static IHostBuilder CreateHostBuilder(/*string[] args*/)
        {
            return Host.CreateDefaultBuilder( /*args*/)
                .ConfigureAppConfiguration((context, builder) =>
                {
                    builder.AddConfiguration(_configuration);
                })
                //.UseSerilog()
                .ConfigureServices((hostContext, services) =>
                {
                    //services.AddSingleton<IEventAggregator, EventAggregator>();

                    //services.AddSingleton<Mpt.Testing.Pre.Wpf.Shared.View.Common.Splash>();

                    services.AddSingleton<MainWindow>();

                    services.AddTransient<KSociety.Log.Serilog.Sinks.Test.WpfApp.ViewModel.LogViewer>();


                })
                .UseServiceProviderFactory(new AutofacServiceProviderFactory(ConfigureContainer));
        }

        private static void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new Bindings.Log(_richTextBoxQueueSink));
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            if (ProcessUtil.PriorProcess() == null)
            {
                var messageBoxText = @"Another instance of the app is already running.";
                var caption = "Conflicting instance";
                var button = MessageBoxButton.OK;
                var icon = MessageBoxImage.Warning;

                var result = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.OK);
                Current.Shutdown(-1);
                return;
            }

            try
            {
                this.ShowSplash();
            }
            catch (Exception ex)
            {
                ;
            }

            base.OnStartup(e);
            var serviceScope = this._host.Services.CreateScope();
            this.ServiceProvider = serviceScope.ServiceProvider;

            DiSource.Resolver = this.Resolve;
        }

        private void ShowSplash()
        {
            // Create the window
            var animatedSplashScreenWindow =
                new KSociety.Log.Serilog.Sinks.Test.WpfApp.Splash();//ServiceProvider.GetRequiredService<Mpt.Testing.Pre.Wpf.Shared.View.Common.Splash>();

            animatedSplashScreenWindow.Show();

            animatedSplashScreenWindow.IsVisibleChanged += this.AnimatedSplashScreenWindowOnIsVisibleChanged;
            // Show it
        }

        private void AnimatedSplashScreenWindowOnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (!(bool)e.NewValue)
            {
                var mainWindows = this.ServiceProvider.GetRequiredService<MainWindow>();

                Application.Current.MainWindow = mainWindows;
                mainWindows.Show();
            }
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            await global::Serilog.Log.CloseAndFlushAsync();
            using (this._host)
            {
                await this._host.StopAsync();
            }

            base.OnExit(e);
        }
    }
}
