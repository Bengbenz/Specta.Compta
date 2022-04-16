using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

using Ardalis.EFCore.Extensions;

using Beng.Specta.Compta.Core.Entities;
using Beng.Specta.Compta.Core.Entities.Auth;
using Beng.Specta.Compta.SharedKernel;
using Beng.Specta.Compta.SharedKernel.Interfaces;

using Finbuckle.MultiTenant;

namespace Beng.Specta.Compta.Infrastructure.Data
{
    public class AppDbContext : MultiTenantIdentityDbContext
    {
        private readonly IDomainEventDispatcher _dispatcher;
        private readonly IConfiguration _configuration;

        public AppDbContext(TenantInfo tenantInfo) : base(tenantInfo)
        {
        }

        public AppDbContext(TenantInfo tenantInfo, IConfiguration configuration) : base(tenantInfo)
        {
            _configuration = configuration;
        }

        public AppDbContext(
            TenantInfo tenantInfo,
            DbContextOptions<AppDbContext> options,
            IConfiguration configuration
            //IDomainEventDispatcher dispatcher
            ) : base(tenantInfo, options)
        {
            //_dispatcher = dispatcher;
            _configuration = configuration;
        }

        public DbSet<Project> Projects { get; set; }

        // Security : Custom authentification/authorisation parts
        public DbSet<UserToRole> UserToRoles { get; set; }

        public DbSet<RoleToPermissions> RolesToPermissions { get; set; }

        public DbSet<ModulesForUser> ModulesForUsers { get; set; }

        /*
         * Don't need with AppDesignTimeFactory
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("AppConnection"));
            base.OnConfiguring(optionsBuilder);
        }*/

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyAllConfigurationsFromCurrentAssembly();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            int result = await base.SaveChangesAsync(cancellationToken);

            // ignore events if no dispatcher provided
            if (_dispatcher == null) return result;

            // dispatch events only if save was successful
            var entitiesWithEvents = ChangeTracker.Entries<BaseEntity>()
                .Select(e => e.Entity)
                .Where(e => e.Events.Any())
                .ToArray();

            foreach (var entity in entitiesWithEvents)
            {
                var events = entity.Events.ToArray();
                entity.Events.Clear();
                foreach (var domainEvent in events)
                {
                    await _dispatcher.Dispatch(domainEvent);
                }
            }

            return result;
        }

        public override int SaveChanges() => SaveChangesAsync().GetAwaiter().GetResult();
    }
}