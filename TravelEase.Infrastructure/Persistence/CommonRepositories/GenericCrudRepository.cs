using Microsoft.EntityFrameworkCore;
using TravelEase.Domain.Common.Interfaces;

namespace TravelEase.Infrastructure.Persistence.CommonRepositories
{
    public class GenericCrudRepository<T> : ICrudRepository<T> where T : class
    {
        protected readonly DbContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericCrudRepository(DbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public virtual async Task<T?> GetByIdAsync(Guid id) => await _dbSet.FindAsync(id);

        public virtual async Task AddAsync(T entity) => await _dbSet.AddAsync(entity);

        public virtual void Update(T entity) => _dbSet.Update(entity);

        public virtual void Remove(T entity) => _dbSet.Remove(entity);
    }
}