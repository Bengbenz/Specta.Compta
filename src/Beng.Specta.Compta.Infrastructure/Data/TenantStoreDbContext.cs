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
            IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(_configuration.GetConnectionString("TenantConnection"));// will be created in server project root
            base.OnConfiguring(optionsBuilder);
        }
    }
}