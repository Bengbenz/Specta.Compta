using System;
using System.Linq;
using System.Net.Mime;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Beng.Specta.Compta.Core.Config;
using Beng.Specta.Compta.Infrastructure.Config;
using Beng.Specta.Compta.Server.Config;
using log4net;

namespace Beng.Specta.Compta.Server
{
    public class Startup
	{
        ///<summary>
        /// Use this method to add services to the container.
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
            
            services.AddDatabaseDeveloperPageExceptionFilter();
            services.AddAppDbContext()
                .AddMultiTenantInfra()
                .AddCustomAuthorization()
                .AddInfrastructureServices()
                .AddAppCoreServices();
        }

        ///<summary>
        /// Use this method to configure the HTTP request pipeline.
        ///</summary>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
			if (env.IsDevelopment())
            {
                app.UseStatusCodePages();
                app.UseWebAssemblyDebugging();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // Middleware for Application_BeginRequest (log)
            app.Use((ctx, next) =>
            {
                LogicalThreadContext.Properties["activityid"] = new ActivityIdHelper(ctx);
                LogicalThreadContext.Properties["requestinfo"] = new WebRequestInfo(ctx);
                return next();
            });

            app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            // Before UseAuthentication !
            app.UseMultiTenant();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            //app.UseBlazorFrameworkFiles();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default", "{__tenant__=}/{controller}/{action}");
                endpoints.MapFallbackToFile("index.html");
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
