using Microsoft.EntityFrameworkCore;

using Finbuckle.MultiTenant.Stores;

namespace Beng.Specta.Compta.Infrastructure.Data
{
    public class TenantStoreDbContext : EFCoreStoreDbContext
    {
        public TenantStoreDbContext(
            DbContextOptions<TenantStoreDbContext> options) : base(options)
        {
        }
    }
}