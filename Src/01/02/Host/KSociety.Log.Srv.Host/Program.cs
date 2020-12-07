using System;
using System.Net;
using System.Threading.Tasks;
using Autofac.Extensions.DependencyInjection;
using KSociety.Log.Serilog.Sinks.RabbitMq.ProtoModel;
using KSociety.Log.Serilog.Sinks.SignalR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Formatting.Json;

namespace KSociety.Log.Srv.Host
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            Configuration.ProtoBufConfiguration();

            //// NLog: setup the logger first to catch all errors
            //var logger = LogManager.GetCurrentClassLogger();

            //Read Configuration from appSettings
            //var config = new ConfigurationBuilder()
            //    .AddJsonFile("appsettings.json")
            //    .Build();

            //Initialize Logger
            //global::Serilog.Log.Logger = new LoggerConfiguration()
            //    .ReadFrom.Configuration(config)
            //    .CreateLogger();

            global::Serilog.Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.File(@"C:\JOB\LOG\log-.txt", rollingInterval: RollingInterval.Day/*, rollOnFileSizeLimit: true*/)
                .WriteTo.SignalR((proxy, signalRConfiguration) =>
                {
                    proxy.Uri = "http://localhost:61000/LoggingHub";

                    signalRConfiguration.TextFormatter = new JsonFormatter();
                    


                }).MinimumLevel.Verbose().CreateLogger();

            try
            {
                //logger.Debug("Log init main");
                global::Serilog.Log.Information("Log init main");
                await CreateHostBuilder(args).Build().RunAsync();

            }
            catch (Exception ex)
            {
                ////NLog: catch setup errors
                //logger.Error(ex, "Stopped program because of exception");
                global::Serilog.Log.Fatal(ex, "Stopped program because of exception");
                throw;
            }
            finally
            {
                //// Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
                //NLog.LogManager.Shutdown();
                global::Serilog.Log.CloseAndFlush();
            }
        }

        //public static IHostBuilder CreateHostBuilder(string[] args) =>
        //    Host.CreateDefaultBuilder(args)
        //        .ConfigureServices((hostContext, services) =>
        //        {
        //            services.AddHostedService<Worker>();
        //        });

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args)
                .UseWindowsService()
                .UseSerilog()
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureKestrel(options =>
                    {
                        options.Listen(IPAddress.Any, 60500);
                    });
                    webBuilder.UseStartup<Startup>();

                });

        //.ConfigureServices((hostContext, services) =>
        //{
        //    services.AddHostedService<Worker>();
        //})
        //.ConfigureLogging(logging =>
        //{
        //    logging.ClearProviders();
        //    logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
        //    //logging.AddFilter("Grpc", LogLevel.None);
        //    //logging.AddFilter("Microsoft", LogLevel.None);
        //    //logging.AddFilter<Microsoft.Extensions.Logging.ILoggerProvider>(level => level == LogLevel.None);
        //});
        //.UseNLog();  // NLog: setup NLog for Dependency injection
    }
}
