using Finbuckle.MultiTenant;
using Finbuckle.MultiTenant.Stores;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Beng.Specta.Compta.Infrastructure.Data
{
    public class TenantStoreDbContext : EFCoreStoreDbContext<TenantInfo>
    {
        private readonly IConfiguration _configuration;

        public TenantStoreDbContext(
            DbContextOptions<TenantStoreDbContext> options,
            IConfiguration configuration)
            : base(options)
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql(_configuration.GetConnectionString("TenantStore"));
            }
            
            base.OnConfiguring(optionsBuilder);
        }
    }
}