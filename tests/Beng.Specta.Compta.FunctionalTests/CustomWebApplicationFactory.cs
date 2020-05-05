using System;
using System.Linq;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Beng.Specta.Compta.Infrastructure.Data.DbContext;
using Beng.Specta.Compta.Infrastructure.Data.Repositories;
using Beng.Specta.Compta.Server;
using Beng.Specta.Compta.SharedKernel.Interfaces;
using Beng.Specta.Compta.UnitTests;

namespace Beng.Specta.Compta.FunctionalTests
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        private static void ReplaceDbContextService<TDbContext>(IServiceCollection services, string connectionString) where TDbContext : DbContext
        {
            // Remove the app's TDbContext registration.
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<TDbContext>));

            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            // Add a database context (TDbContext) using an in-memory
            // database for testing.
            services.AddDbContext<TDbContext>(options =>
            {
                options.UseInMemoryDatabase(connectionString);
            });
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder?.ConfigureServices(services =>
            {
                ReplaceDbContextService<AppDbContext>(services, "Beng.Specta.Compta.FonctionalTest");
                ReplaceDbContextService<TenantStoreDbContext>(services, "Beng.Specta.TenantStore.FonctionalTest");

                services.AddScoped<IDomainEventDispatcher, NoOpDomainEventDispatcher>();
                services.AddScoped<IRepository, EfRepository>();

                // Build the service provider.
                var sp = services.BuildServiceProvider();

                // Create a scope to obtain a reference to the database
                // context (AppDbContext).
                using (var scope = sp.CreateScope())
                {
                    var serviceProvider = scope.ServiceProvider;
                    try
                    {
                        // Seed the database with test data.
                        SeedData.PopulateDefaultTenantInfo(serviceProvider);
                        SeedData.PopulateAppDatabase(serviceProvider);
                    }
                    catch (Exception ex)
                    {
                        var logger = serviceProvider.GetRequiredService<ILogger<CustomWebApplicationFactory<TStartup>>>();
                        logger.LogError(ex, "An error occurred seeding the " +
                            $"database with test messages. Error: {ex.Message}");
                    }
                }
            });
        }
    }
}
