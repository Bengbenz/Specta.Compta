using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Beng.Specta.Compta.Infrastructure.Data;

/// <summary>
///     Used for design time migrations.  
///     Will look to the appsettings.json file in this project for the connection string.
///     EF Core tools scans the assembly containing the dbcontext for an implementation
///     of IDesignTimeDbContextFactory.
/// </summary>
public class TenantStoreDesignTimeFactory : IDesignTimeDbContextFactory<TenantStoreDbContext>
{
    public TenantStoreDbContext CreateDbContext(string[] args)
    {
        // Get environment
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

        // Build config
        IConfiguration config = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../Beng.Specta.Compta.Server"))
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{environment}.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<TenantStoreDbContext>();
        optionsBuilder.UseNpgsql(config.GetConnectionString("TenantStore"));

        return new TenantStoreDbContext(optionsBuilder.Options, config);
    }
}