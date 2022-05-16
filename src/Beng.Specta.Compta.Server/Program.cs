using Microsoft.EntityFrameworkCore;

using Beng.Specta.Compta.Infrastructure.Data;
using Beng.Specta.Compta.Server.Config;
using Finbuckle.MultiTenant;
using log4net;

namespace Beng.Specta.Compta.Server
{
    public sealed class Program
    {
        public static async Task Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
            builder.Host.ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                logging.AddConsole();
                logging.AddLog4Net("log4Net.xml");
            });
            
            // Add services to the container.
            builder.Services.ConfigureServices();
            
            var app = builder.Build();
            
            // Configure the HTTP request pipeline.
            ConfigureMiddleware(app, app.Environment);
            
            // Seed App Database
            //a) migrate/create the databases
            var logger = app.Services.GetRequiredService<ILogger<Program>>();
            await MigrateTenantDatabaseAsync(app.Services, logger);
            await MigrateAppDatabasesAsync(app.Services, logger);
            
            //b) Add a Default Tenant and SuperUser, so you can log in and see what happens
            await SeedData.PopulateAppDatabaseAsync(app.Services, logger);
            
            // Run Server
            await app.RunAsync();
        }

        private static async Task MigrateTenantDatabaseAsync(IServiceProvider serviceProvider, ILogger logger)
        {
            try
            {
                using var scope = serviceProvider.CreateScope();
                var scopeServiceProvider = scope.ServiceProvider;
                await using var tenantContext = scopeServiceProvider.GetRequiredService<TenantStoreDbContext>();
                if (!tenantContext.Database.ProviderName!.Contains("Microsoft.EntityFrameworkCore.InMemory"))
                {
                    await tenantContext.Database.MigrateAsync();
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"An error occurred seeding the App DBs. Error: {ex.Message}");
            }
        }

        private static async Task MigrateAppDatabasesAsync(IServiceProvider serviceProvider, ILogger logger)
        {
            try
            {
                using var scope = serviceProvider.CreateScope();
                var scopeServiceProvider = scope.ServiceProvider;
                var store = scopeServiceProvider.GetRequiredService<IMultiTenantStore<TenantInfo>>();
                foreach (var tenant in await store.GetAllAsync())
                {
                    await using var dbContext = new AppDbContext(tenant, scopeServiceProvider.GetRequiredService<DbContextOptions<AppDbContext>>());
                    if (!dbContext.Database.ProviderName!.Contains("Microsoft.EntityFrameworkCore.InMemory"))
                    {
                        await dbContext.Database.MigrateAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"An error occurred seeding the Tenant DBs. Error: {ex.Message}");
            }
        }
        
        ///<summary>
        /// Use this method to configure the HTTP request pipeline.
        ///</summary>
        private static void ConfigureMiddleware(IApplicationBuilder app, IWebHostEnvironment env)
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
                LogicalThreadContext.Properties["activityid"] = new ActivityIdInfo(ctx);
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

        private class ActivityIdInfo
        {
            private readonly string _activityId;

            public ActivityIdInfo(HttpContext context)
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
