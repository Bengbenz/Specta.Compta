using System;
using System.IO;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

using Finbuckle.MultiTenant;

namespace Beng.Specta.Compta.Infrastructure.Data
{
    public class AppDesignTimeFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
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

            IConfigurationSection configSection = config.GetSection("DefaultTenant");
            var tenantInfo = new TenantInfo(configSection.GetValue<string>("Id"),
                                            configSection.GetValue<string>("Identifier"),
                                            configSection.GetValue<string>("Name"),
                                            configSection.GetValue<string>("ConnectionString"),
                                            null);
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

            return new AppDbContext(tenantInfo, optionsBuilder.Options, config);
        }
    }
}
