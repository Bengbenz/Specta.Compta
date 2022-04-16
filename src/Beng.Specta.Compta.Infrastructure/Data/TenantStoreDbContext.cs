using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

using Finbuckle.MultiTenant.Stores;

namespace Beng.Specta.Compta.Infrastructure.Data
{
    public class TenantStoreDbContext : EFCoreStoreDbContext
    {
        private readonly IConfiguration _configuration;

        public TenantStoreDbContext(
            DbContextOptions<TenantStoreDbContext> options,
            IConfiguration configuration = null)
            : base(options)
        {
            _configuration = configuration;
        }

        /*
         * Don't need with TenantStoreDesignTimeFactory
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("TenantConnection"));
            base.OnConfiguring(optionsBuilder);
        }*/
    }
}