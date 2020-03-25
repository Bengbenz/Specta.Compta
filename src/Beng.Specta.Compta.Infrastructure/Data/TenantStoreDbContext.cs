using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

using Finbuckle.MultiTenant.Stores;

namespace Beng.Specta.Compta.Infrastructure.Data
{
    public class TenantStoreDbContext : EFCoreStoreDbContext
    {
        private IConfiguration Configuration { get; }

        public TenantStoreDbContext(
            DbContextOptions<TenantStoreDbContext> options,
            IConfiguration configuration) : base(options)
        {
            Configuration = configuration;
        }

/*
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // optionsBuilder.UseSqlite(Configuration.GetConnectionString("TenantConnection"));
            optionsBuilder.UseSqlite("Data Source=TenantStoreDb.sqlite"); // will be created in web project root
            base.OnConfiguring(optionsBuilder);
        }*/
    }
}