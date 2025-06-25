using TravelEase.Domain.Common.Pagination;

namespace TravelEase.Domain.Common.Interfaces
{
    public interface IReadableRepository<T> where T : class
    {
        Task<PaginatedList<T>> GetAllAsync(int pageNumber, int pageSize);
    }
}