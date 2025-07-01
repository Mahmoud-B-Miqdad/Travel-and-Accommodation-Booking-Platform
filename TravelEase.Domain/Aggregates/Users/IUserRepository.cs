using TravelEase.Domain.Common.Interfaces;
using TravelEase.Domain.Common.Models.PaginationModels;

namespace TravelEase.Domain.Aggregates.Users
{
    public interface IUserRepository : ICrudRepository<User>
    {
        Task<PaginatedList<User>> GetAllAsync(bool includeBookings, int pageNumber, int pageSize);
    }
}