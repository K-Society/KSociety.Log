using KSociety.Log.Pre.Web.App;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .MinimumLevel.Verbose().CreateLogger();

try
{
    Log.Information("Log init main");
    await CreateHostBuilder(args).Build().RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Stopped program because of exception");
    throw;
}
finally
{
    Log.CloseAndFlush();
}


static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .UseSystemd()
        .UseWindowsService()
        .UseSerilog()
        .ConfigureWebHostDefaults(webBuilder =>
        {
            /*webBuilder.ConfigureKestrel(options =>
            {
                options.Listen(IPAddress.Any, 61000);
            });*/
            webBuilder.UseStartup<Startup>();
        });
