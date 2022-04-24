using System;
using System.IO;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

using Finbuckle.MultiTenant;

namespace Beng.Specta.Compta.Infrastructure.Data
{
    /// <summary>
    ///     Used for design time migrations.  
    ///     Will look to the appsettings.json file in this project for the connection string.
    ///     EF Core tools scans the assembly containing the dbcontext for an implementation
    ///     of IDesignTimeDbContextFactory.
    /// </summary>
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
            var tenantInfo = new TenantInfo
            {
                Id = configSection.GetValue<string>("Id"),
                Identifier = configSection.GetValue<string>("Identifier"),
                Name = configSection.GetValue<string>("Name"),
                ConnectionString = configSection.GetValue<string>("ConnectionString")
            };
            
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseNpgsql(tenantInfo.ConnectionString);

            return new AppDbContext(tenantInfo, optionsBuilder.Options);
        }
    }
}
