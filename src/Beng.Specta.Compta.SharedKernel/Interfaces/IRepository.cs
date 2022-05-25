namespace Beng.Specta.Compta.SharedKernel.Interfaces;

public interface IRepository
{
    Task<T?> GetByIdAsync<T>(long id) where T : BaseEntity;

    Task<IReadOnlyCollection<T>> ListAsync<T>() where T : class;

    Task AddAsync<T>(params T[] entities) where T : class;

    Task UpdateAsync<T>(T entity) where T : class; 

    Task DeleteAsync<T>(params T[] entities) where T : class;
}