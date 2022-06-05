using Beng.Specta.Compta.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Beng.Specta.Compta.SharedKernel;
using Beng.Specta.Compta.SharedKernel.Interfaces;

namespace Beng.Specta.Compta.Infrastructure.Repositories;

public class GenericRepository : IRepository
{
    protected AppDbContext DbContext { get; }
    protected ILogger Logger { get; }

    public GenericRepository(AppDbContext dbContext, ILogger<IRepository> logger)
    {
        DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        Logger = logger;
    }

    public Task<T?> GetByIdAsync<T>(long id) where T : BaseEntity
    {
        return DbContext.Set<T>().SingleOrDefaultAsync(e => e.Id == id);
    }

    public async Task<IReadOnlyCollection<T>> ListAsync<T>() where T : BaseEntity
    {
        return await DbContext.Set<T>().ToListAsync();
    }
    
    public async Task<int> CountAsync<T>() where T : BaseEntity
    {
        return await DbContext.Set<T>().CountAsync();
    }
    
    public async Task<bool> AnyAsync<T>() where T : BaseEntity
    {
        return await DbContext.Set<T>().AnyAsync();
    }
    
    public async Task AddAsync<T>(params T[] entities) where T : class
    {
        ArgumentNullException.ThrowIfNull(entities);
        await DbContext.Set<T>().AddRangeAsync(entities);
        await DbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync<T>(T entity) where T : BaseEntity
    {
        ArgumentNullException.ThrowIfNull(entity);
        DbContext.Entry(entity).State = EntityState.Modified;
        await DbContext.SaveChangesAsync();
    }
    
    public async Task DeleteAsync<T>(params T[] entities) where T : class
    {
        ArgumentNullException.ThrowIfNull(entities);
        DbContext.Set<T>().RemoveRange(entities);
        await DbContext.SaveChangesAsync();
    }
}