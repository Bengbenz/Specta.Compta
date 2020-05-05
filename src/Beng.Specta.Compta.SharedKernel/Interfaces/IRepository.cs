using System.Collections.Generic;
using System.Threading.Tasks;

namespace Beng.Specta.Compta.SharedKernel.Interfaces
{
    public interface IRepository
    {
        T GetById<T>(long id) where T : BaseEntity;

        List<T> List<T>() where T : BaseEntity;

        T Add<T>(T entity) where T : BaseEntity;

        Task<T> AddAsync<T>(T entity) where T : BaseEntity;

        void Update<T>(T entity) where T : BaseEntity;

        Task DeleteAsync<T>(T entity) where T : BaseEntity;

        Task DeleteRangeAsync<T>(IEnumerable<T> entity) where T : BaseEntity;
    }
}