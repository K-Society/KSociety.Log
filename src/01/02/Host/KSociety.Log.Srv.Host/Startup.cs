// Copyright � K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace KSociety.Log.Srv.Host
{
    using System;
    using System.IO.Compression;
    using Autofac;
    using global::Serilog;
    using KSociety.Base.InfraSub.Shared.Class;
    using KSociety.Base.Srv.Host.Shared.Bindings;
    using KSociety.Log.Biz.Interface;
    using KSociety.Log.Srv.Behavior.Biz;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Server.Kestrel.Core;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using ProtoBuf.Grpc.Server;

    public class Startup
    {
        public IHostApplicationLifetime AppLifetime { get; private set; }
        public static ILifetimeScope AutofacContainer { get; private set; }


        private bool DebugFlag { get; }
        private Base.EventBusRabbitMQ.Binding.MessageBrokerOptions MessageBrokerOptions { get; }

        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;

            this.DebugFlag = this.Configuration.GetValue<bool>("DebugFlag");

            this.MessageBrokerOptions = this.Configuration.GetSection("MessageBroker").Get<Base.EventBusRabbitMQ.Binding.MessageBrokerOptions>();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<KestrelServerOptions>(
                this.Configuration.GetSection("Kestrel"));

            //services.AddCodeFirstGrpc();
            services.AddCodeFirstGrpc(options =>
            {
                options.ResponseCompressionAlgorithm = @"gzip";
                options.ResponseCompressionLevel = CompressionLevel.Optimal;
            });
        }

        // ConfigureContainer is where you can register things directly
        // with Autofac. This runs after ConfigureServices so the things
        // here will override registrations made in ConfigureServices.
        // Don't build the container; that gets done for you by the factory.
        public void ConfigureContainer(ContainerBuilder builder)
        {
            try
            {
                //Log.
                builder.RegisterModule(new KSociety.Base.Srv.Host.Shared.Bindings.Log());

                //AutoMapper.
                //builder.RegisterModule(
                //    new KSociety.Base.Srv.Host.Shared.Bindings.AutoMapper(AssemblyTool.GetAssembly()));

                //CommandHdlr.
                builder.RegisterModule(new CommandHdlr(AssemblyTool.GetAssembly()));

                //RabbitMQ.
                builder.RegisterModule(new KSociety.Log.EventBus.MessageBroker(this.MessageBrokerOptions, this.DebugFlag));

                //Transaction, don't move this line.
                builder.RegisterModule(new Bindings.Biz.Biz(this.DebugFlag));

            }
            catch (Exception ex)
            {
                Console.WriteLine(@"Autofac ConfigureContainer: " + ex.Message + " " + ex.StackTrace);
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime appLifetime,
            IHostLifetime hostLifetime, ILifetimeScope autofacContainer)
        {
            this.AppLifetime = appLifetime;
            AutofacContainer = autofacContainer;

            appLifetime.ApplicationStarted.Register(OnStarted);
            appLifetime.ApplicationStopping.Register(OnStopping);
            appLifetime.ApplicationStopped.Register(OnStopped);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSerilogRequestLogging();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<Behavior.Biz.Biz>();
                endpoints.MapGrpcService<BizAsync>();

                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync(
                        "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
                });
            });
        }

        private static void OnStarted()
        {
            // Perform post-startup activities here
            Console.WriteLine(@"OnStarted: ");
        }

        private static void OnStopping()
        {
            // Perform on-stopping activities here
            Console.WriteLine(@"OnStopping: ");
            try
            {
                if (AutofacContainer.IsRegistered<IBiz>())
                {
                    var biz = AutofacContainer.Resolve<IBiz>();
                    //biz.DestroyAll();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(@"OnStopping: " + ex.Message);
            }
        }

        private static void OnStopped()
        {
            // Perform post-stopped activities here
            Console.WriteLine(@"OnStopped: ");
        }
    }
}
