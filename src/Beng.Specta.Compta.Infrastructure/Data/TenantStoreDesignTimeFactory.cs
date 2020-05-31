using System;
using System.IO;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
namespace Beng.Specta.Compta.Infrastructure.Data
{
    public class TenantStoreDesignTimeFactory : IDesignTimeDbContextFactory<TenantStoreDbContext>
    {
        public TenantStoreDbContext CreateDbContext(string[] args)
        {
            // Get environment
            string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            // Build config
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../Beng.Specta.Compta.Server"))
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<TenantStoreDbContext>();

            return new TenantStoreDbContext(optionsBuilder.Options, config);
        }
    }
}
