using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Beng.Specta.Compta.SharedKernel;
using Beng.Specta.Compta.SharedKernel.Interfaces;

namespace Beng.Specta.Compta.Infrastructure.Data.Repositories
{
    public class EfRepository : IRepository
    {
        protected AppDbContext DbContext { get; }
        protected ILogger Logger { get; }

        public EfRepository(AppDbContext dbContext, ILogger<EfRepository> logger = null)
        {
            DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            Logger = logger;
        }

        public Task<T> GetByIdAsync<T>(long id) where T : BaseEntity
        {
            return DbContext.Set<T>().SingleOrDefaultAsync(e => e.Id == id);
        }

        public async Task<ICollection<T>> ListAsync<T>() where T : class
        {
            return await DbContext.Set<T>().ToListAsync();
        }

        public async Task AddAsync<T>(params T[] entities) where T : class
        {
            await DbContext.Set<T>().AddRangeAsync(entities);
            await DbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync<T>(T entity) where T : class
        {
            DbContext.Entry(entity).State = EntityState.Modified;
            await DbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync<T>(params T[] entities) where T : class
        {
            DbContext.Set<T>().RemoveRange(entities);
            await DbContext.SaveChangesAsync();
        }
    }
}
