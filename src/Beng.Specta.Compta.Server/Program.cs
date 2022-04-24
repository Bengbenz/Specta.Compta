using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Beng.Specta.Compta.Infrastructure.Data;
using Finbuckle.MultiTenant;

namespace Beng.Specta.Compta.Server
{
    public sealed class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Host.ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                logging.AddConsole();
                logging.AddLog4Net("log4Net.xml");
            });

            var startup = new Startup();
            
            // Add services to the container.
            startup.ConfigureServices(builder.Services);
            
            var app = builder.Build();
            
            // Configure the HTTP request pipeline.
            startup.Configure(app, app.Environment);
            
            // Seed App Database
            //a) migrate/create the databases
            var logger = app.Services.GetRequiredService<ILogger<Program>>();
            await MigrateDatabasesAsync(app.Services, logger);
            
            //b) Add a Default Tenant and SuperUser, so you can log in and see what happens
            await SeedData.PopulateAppDatabaseAsync(app.Services, logger);

            // Run Server
            await app.RunAsync();
        }

        private static async Task MigrateDatabasesAsync(IServiceProvider serviceProvider, ILogger logger)
        {
            try
            {
                var scopeServices = serviceProvider.CreateScope().ServiceProvider;
                await using var tenantContext = scopeServices.GetRequiredService<TenantStoreDbContext>();
                await tenantContext.Database.MigrateAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"An error occurred seeding the App DBs. Error: {ex.Message}");
            }

            try
            {
                var scopeServices = serviceProvider.CreateScope().ServiceProvider;
                var store = scopeServices.GetRequiredService<IMultiTenantStore<TenantInfo>>();
                foreach(var tenant in await store.GetAllAsync())
                {
                    await using var db = new AppDbContext(tenant);
                    await db.Database.MigrateAsync();
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"An error occurred seeding the App DBs. Error: {ex.Message}");
            }
        }
    }
}
