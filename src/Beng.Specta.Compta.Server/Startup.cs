﻿using System;
using System.Linq;
using System.Net.Mime;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Beng.Specta.Compta.Infrastructure;

using log4net;

namespace Beng.Specta.Compta.Server
{
    public class Startup
	{
		public IConfiguration Configuration { get; }

        public IWebHostEnvironment Env { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Env = env;
        }

        ///<summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        ///</summary>
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddMvc()
                    .AddNewtonsoftJson();

            services.AddResponseCompression(options =>
            {
                options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
                    new[] { MediaTypeNames.Application.Octet });
            });
            
            services.AddDbContext()
                    .AddMultiTenantInfra()
                    .AddCustomAuthorization()
                    .AddInfrastructureServices();
                    //.AddAppWebServices(typeof(Startup).Assembly);
        }

        ///<summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        ///</summary>
        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddLog4Net("log4Net.xml");

			if (Env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseStatusCodePages();
                app.UseBlazorDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // Middlware for Application_BeginRequest (log)
            app.Use((ctx, next) =>
            {
                LogicalThreadContext.Properties["activityid"] = new ActivityIdHelper(ctx);
                LogicalThreadContext.Properties["requestinfo"] = new WebRequestInfo(ctx);
                return next();
            });

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            // In ASP.NET Core 3 this should be before UseMultiTenant!
            app.UseRouting();

            // Before UseAuthentication and UseMvc!
            app.UseMultiTenant();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseClientSideBlazorFiles<Client.Startup>();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default", "{__tenant__=}/{controller}/{action}");
                //endpoints.MapControllers();
                endpoints.MapDefaultControllerRoute();
                endpoints.MapFallbackToClientSideBlazor<Client.Startup>("index.html");
            });
		}

        private class ActivityIdHelper
        {
            private readonly string _activityId;

            public ActivityIdHelper(HttpContext context)
            {
                if (context == null) throw new ArgumentNullException(nameof(context));

                _activityId = context.TraceIdentifier;
            }

            public override string ToString() => _activityId;
        }

        private class WebRequestInfo
        {
            private readonly string _message;

            public WebRequestInfo(HttpContext context)
            {
                if (context == null) throw new ArgumentNullException(nameof(context));

                var userAgent = context.Request.Headers["User-Agent"];
                _message = $"{context.Request.Path}, {userAgent}";
            }

            public override string ToString() => _message;
        }
	}
}
