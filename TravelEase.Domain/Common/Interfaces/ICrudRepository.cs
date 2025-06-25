namespace TravelEase.Domain.Common.Interfaces
{
    public interface ICrudRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(Guid id);
        Task AddAsync(T entity);
        void Update(T entity);
        void Remove(T entity);
    }
}