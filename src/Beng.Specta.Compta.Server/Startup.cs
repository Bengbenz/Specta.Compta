using System;
using System.Linq;
using System.Net.Mime;

using log4net;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Beng.Specta.Compta.Infrastructure;
using Beng.Specta.Compta.Infrastructure.Data;
using Microsoft.AspNetCore.Http;

namespace Beng.Specta.Compta.Server
{
    public class Startup
	{
		public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Env = env;
        }
		public IConfiguration Configuration { get; }

        public IWebHostEnvironment Env { get; }

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

            //Tenants Store
            services.AddDbContext<TenantStoreDbContext>(options =>
                options.UseSqlite(Configuration.GetConnectionString("TenantConnection")));

            // Add Beng.Specta.Compta app service from infrastructure
            services.AddDbContext()
                    .AddMultiTenantInfra();
                    //.AddRepository();

            //Allows accessing HttpContext in Blazor
            services.AddHttpContextAccessor();
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

            // app.UseAuthentication();
            // app.UseAuthorization();

            app.UseClientSideBlazorFiles<Client.Startup>();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default", "{__tenant__=}/{controller}/{action}");
                endpoints.MapControllers();
                endpoints.MapFallbackToClientSideBlazor<Client.Startup>("index.html");
            });

            // Seed the database the multitenant store will need.
            InitDefaultTenant(app.ApplicationServices);
		}

        private void InitDefaultTenant(IServiceProvider serviceProvider)
        {
            var scopeServices = serviceProvider.CreateScope().ServiceProvider;
            try
            {
                var dbContext = scopeServices.GetRequiredService<TenantStoreDbContext>();
                if(dbContext.Database.EnsureCreated())
                {
                    SeedData.InitDefaultTenantInfo(dbContext);
                }
            }
            catch (Exception ex)
            {
                var logger = scopeServices.GetRequiredService<ILogger<Startup>>();
                logger.LogError(ex, "An error occurred on adding the default tenannt in the DB.");
            }
        }

        public class ActivityIdHelper
        {
            private readonly string _activityId;

            public ActivityIdHelper(HttpContext ctx)
            {
                _activityId = ctx.TraceIdentifier;
            }

            public override string ToString() => _activityId;
        }

        public class WebRequestInfo
        {
            private readonly string _message;

            public WebRequestInfo(HttpContext context)
            {
                var userAgent = context.Request.Headers["User-Agent"];
                _message = $"{context.Request.Path}, {userAgent}";
            }

            public override string ToString() => _message;
        }
	}
}
