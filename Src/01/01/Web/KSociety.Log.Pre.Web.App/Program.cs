using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace KSociety.Log.Pre.Web.App
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            //Read Configuration from appSettings
            //var config = new ConfigurationBuilder()
            //    .AddJsonFile("appsettings.json")
            //    .Build();

            ////Initialize Logger
            //global::Serilog.Log.Logger = new LoggerConfiguration()
            //    .ReadFrom.Configuration(config)
            //    .CreateLogger();

            //CreateHostBuilder(args).Build().Run();

            global::Serilog.Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .MinimumLevel.Verbose().CreateLogger();

            try
            {
                global::Serilog.Log.Information("Log init main");
                await CreateHostBuilder(args).Build().RunAsync();

            }
            catch (Exception ex)
            {
                global::Serilog.Log.Fatal(ex, "Stopped program because of exception");
                throw;
            }
            finally
            {
                global::Serilog.Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseWindowsService()
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureKestrel(options =>
                    {
                        options.Listen(IPAddress.Any, 61000);
                    });
                    webBuilder.UseStartup<Startup>();
                });
    }
}
