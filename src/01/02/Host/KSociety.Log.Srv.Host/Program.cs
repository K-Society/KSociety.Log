// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

using Autofac.Extensions.DependencyInjection;
using KSociety.Log.Serilog.Sinks.RabbitMq.ProtoModel;
using KSociety.Log.Srv.Host;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;

Configuration.ProtoBufConfiguration();

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(configuration).CreateLogger();

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
    // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
    Log.CloseAndFlush();
}

static IHostBuilder CreateHostBuilder(string[] args)
{
    return Host.CreateDefaultBuilder(args)
        .UseSystemd()
        .UseWindowsService()
        .UseSerilog()
        .UseServiceProviderFactory(new AutofacServiceProviderFactory())
        .ConfigureWebHostDefaults(webBuilder =>
        {
            webBuilder.UseStartup<Startup>();
        });
}
