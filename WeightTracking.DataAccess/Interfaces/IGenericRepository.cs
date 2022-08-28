using System;
namespace WeightTracking.DataAccess.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task InsertAsync(T entity);

        void Delete(T entity);

        IQueryable<T> AsQueryable();

        Task SaveChangesAsync();

        Task<T> GetByIdAsync(int? id);
    }
}

