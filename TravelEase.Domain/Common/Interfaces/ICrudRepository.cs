namespace TravelEase.Domain.Common.Interfaces
{
    public interface ICrudRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(Guid id);
        Task<T> AddAsync(T entity);
        void Update(T entity);
        void Remove(T entity);
        Task<bool> IsExistsAsync(Guid id);
        Task<bool> IsExistsAsync(string name);
    }
}