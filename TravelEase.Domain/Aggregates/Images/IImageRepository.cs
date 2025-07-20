using System.Linq.Expressions;
using TravelEase.Domain.Common.Interfaces;
using TravelEase.Domain.Common.Models.PaginationModels;

namespace TravelEase.Domain.Aggregates.Images
{
    public interface IImageRepository : ICrudRepository<Image>
    {
        Task<PaginatedList<string>> GetAllImageUrlsByEntityIdAsync
            (Guid entityId, int pageNumber, int pageSize);
        Task<Image?> GetSingleOrDefaultAsync(Expression<Func<Image, bool>> predicate);
    }
}