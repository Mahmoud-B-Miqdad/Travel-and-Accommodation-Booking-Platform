using TravelEase.Domain.Common.Models.PaginationModels;

namespace TravelEase.Domain.Aggregates.Users
{
    public interface IUserRepository
    {
        Task<PaginatedList<User>> GetAllAsync(bool includeBookings, int pageNumber, int pageSize);
    }
}