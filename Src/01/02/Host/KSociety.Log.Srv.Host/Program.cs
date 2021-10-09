using Autofac.Extensions.DependencyInjection;
using KSociety.Log.Serilog.Sinks.RabbitMq.ProtoModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Threading.Tasks;

namespace KSociety.Log.Srv.Host
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            Configuration.ProtoBufConfiguration();

            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            global::Serilog.Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(configuration).CreateLogger();

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
                // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
                global::Serilog.Log.CloseAndFlush();
            }
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args)
                .UseSystemd()
                .UseWindowsService()
                .UseSerilog()
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
