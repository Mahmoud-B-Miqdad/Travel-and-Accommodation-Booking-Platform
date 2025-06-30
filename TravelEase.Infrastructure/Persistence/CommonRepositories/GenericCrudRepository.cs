using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
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

        public virtual async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            return entity;
        }

        public virtual void Update(T entity) => _dbSet.Update(entity);

        public virtual void Remove(T entity) => _dbSet.Remove(entity);

        public virtual async Task<bool> IsExistsAsync(Guid id)
        {
            return await _dbSet
                .AsNoTracking()
                .AnyAsync(e => EF.Property<Guid>(e, "Id") == id);
        }

        public virtual async Task<bool> IsExistsAsync(string name)
        {
            var propertyInfo = typeof(T).GetProperty("Name");
            if (propertyInfo == null || propertyInfo.PropertyType != typeof(string))
                throw new InvalidOperationException($"Type {typeof(T).Name} does not contain a string property named 'Name'.");

            var parameter = Expression.Parameter(typeof(T), "e");
            var property = Expression.Property(parameter, propertyInfo);
            var constant = Expression.Constant(name);
            var equal = Expression.Equal(property, constant);
            var lambda = Expression.Lambda<Func<T, bool>>(equal, parameter);

            return await _dbSet.AnyAsync(lambda);
        }
    }
}