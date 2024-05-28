// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace KSociety.Log.Pre.Web.App
{
    using KSociety.Log.Pre.Web.App.Hubs;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Server.Kestrel.Core;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Serilog;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<KestrelServerOptions>(
                this.Configuration.GetSection("Kestrel"));

            services.AddControllersWithViews();
            //services.AddCors(options => options.AddPolicy("CorsPolicy",
            //    builder =>
            //    {
            //        builder.AllowAnyMethod().AllowAnyHeader()
            //            .WithOrigins("http://localhost:51305")
            //            .AllowCredentials();
            //    }));
            services.AddSignalR();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            //app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseSerilogRequestLogging();

            app.UseRouting();

            app.UseAuthorization();

            //app.UseCors("CorsPolicy");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapHub<LoggingHub>("/loggingHub");

                //endpoints.MapHub<LogHub>("/log");
            });

            //global::Serilog.Log.Logger = new LoggerConfiguration().
            //    CreateLoggerFromConfig(Configuration, app.ApplicationServices);
        }
    }
}