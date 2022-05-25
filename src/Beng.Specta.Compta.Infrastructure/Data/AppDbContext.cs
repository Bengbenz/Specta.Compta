using Microsoft.EntityFrameworkCore;

using Ardalis.EFCore.Extensions;

using Beng.Specta.Compta.Core.Entities;
using Beng.Specta.Compta.Core.Entities.Identities;
using Beng.Specta.Compta.SharedKernel;
using Beng.Specta.Compta.SharedKernel.Interfaces;

using Finbuckle.MultiTenant;

namespace Beng.Specta.Compta.Infrastructure.Data;

public class AppDbContext : MultiTenantIdentityDbContext
{
    private readonly IDomainEventDispatcher? _dispatcher;

    public AppDbContext(
        ITenantInfo tenantInfo) : base(tenantInfo)
    {
    }

    public AppDbContext(
        ITenantInfo tenantInfo,
        DbContextOptions<AppDbContext> options,
        IDomainEventDispatcher? dispatcher = null) : base(tenantInfo, options)
    {
        _dispatcher = dispatcher;
    }

    public DbSet<Project> Projects { get; set; } = null!;

    // Security : Custom authentication/authorisation parts
    public DbSet<UserToRole> UserToRoles { get; set; } = null!;

    public DbSet<RoleToPermissions> RolesToPermissions { get; set; } = null!;

    public DbSet<ModulesForUser> ModulesForUsers { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseNpgsql(TenantInfo.ConnectionString ?? throw new InvalidOperationException());
        }
            
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyAllConfigurationsFromCurrentAssembly();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var result = await base.SaveChangesAsync(cancellationToken);

        // ignore events if no dispatcher provided
        if (_dispatcher is null)
        {
            return result;
        }

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