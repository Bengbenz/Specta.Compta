using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using Ardalis.EFCore.Extensions;

using Beng.Specta.Compta.Core.Entities;
using Beng.Specta.Compta.SharedKernel;
using Beng.Specta.Compta.SharedKernel.Interfaces;

using Finbuckle.MultiTenant;

namespace Beng.Specta.Compta.Infrastructure.Data {
    public class AppDbContext : MultiTenantDbContext
    {
        private readonly IDomainEventDispatcher _dispatcher;

        public AppDbContext(TenantInfo tenantInfo) : base(tenantInfo)
        {
        }

        public AppDbContext(
            TenantInfo tenantInfo,
            DbContextOptions<AppDbContext> options
            //IDomainEventDispatcher dispatcher
            ) : base(tenantInfo, options)
        {
            //_dispatcher = dispatcher;
        }

        public DbSet<ToDoItem> ToDoItems { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Use Sqlite, but could be MsSql, MySql, Postgre, InMemory etc...
            optionsBuilder.UseSqlite(ConnectionString); 
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyAllConfigurationsFromCurrentAssembly();

            base.OnModelCreating(modelBuilder);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            int result = await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

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
                    await _dispatcher.Dispatch(domainEvent).ConfigureAwait(false);
                }
            }

            return result;
        }

        public override int SaveChanges() => SaveChangesAsync().GetAwaiter().GetResult();
    }
}