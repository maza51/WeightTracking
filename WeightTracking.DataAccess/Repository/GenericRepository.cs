using System;
using WeightTracking.DataAccess.Interfaces;

namespace WeightTracking.DataAccess.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly AppDbContext _dbContext;

        public GenericRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<T> AsQueryable()
        {
            return _dbContext.Set<T>().AsQueryable();
        }

        public void Delete(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
        }

        public async Task InsertAsync(T entity)
        {
            await _dbContext.AddAsync(entity);
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task<T> GetByIdAsync(int? id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }
    }
}

