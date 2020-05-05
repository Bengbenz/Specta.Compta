using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using Beng.Specta.Compta.Infrastructure.Data.DbContext;
using Beng.Specta.Compta.SharedKernel;
using Beng.Specta.Compta.SharedKernel.Interfaces;

namespace Beng.Specta.Compta.Infrastructure.Data.Repositories
{
    public class EfRepository : IRepository
    {
        protected AppDbContext DbContext { get; }

        public EfRepository(AppDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public T GetById<T>(long id) where T : BaseEntity
        {
            return DbContext.Set<T>().SingleOrDefault(e => e.Id == id);
        }

        public List<T> List<T>() where T : BaseEntity
        {
            return DbContext.Set<T>().ToList();
        }

        public T Add<T>(T entity) where T : BaseEntity
        {
            DbContext.Set<T>().Add(entity);
            DbContext.SaveChanges();

            return entity;
        }

        public async Task<T> AddAsync<T>(T entity) where T : BaseEntity
        {
            DbContext.Set<T>().Add(entity);
            await DbContext.SaveChangesAsync();

            return entity;
        }

        public void Update<T>(T entity) where T : BaseEntity
        {
            DbContext.Entry(entity).State = EntityState.Modified;
            DbContext.SaveChanges();
        }

        public async Task DeleteAsync<T>(T entity) where T : BaseEntity
        {
            DbContext.Set<T>().Remove(entity);
            await DbContext.SaveChangesAsync();
        }

        public async Task DeleteRangeAsync<T>(IEnumerable<T> entities) where T : BaseEntity
        {
            DbContext.Set<T>().RemoveRange(entities);
            await DbContext.SaveChangesAsync();
        }
    }
}
