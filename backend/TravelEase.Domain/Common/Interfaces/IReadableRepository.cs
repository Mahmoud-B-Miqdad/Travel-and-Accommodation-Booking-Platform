using TravelEase.Domain.Common.Models.PaginationModels;

namespace TravelEase.Domain.Common.Interfaces
{
    public interface IReadableRepository<T> where T : class
    {
        Task<PaginatedList<T>> GetAllAsync(int pageNumber, int pageSize);
    }
}