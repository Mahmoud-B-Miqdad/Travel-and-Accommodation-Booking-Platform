using System.Linq.Expressions;
using TravelEase.Domain.Common.Interfaces;

namespace TravelEase.Domain.Aggregates.Images
{
    public interface IImageRepository : ICrudRepository<Image>
    {
        Task<List<string>> GetAllImageUrlsByEntityIdAsync(Guid entityId);
        Task<Image?> GetSingleOrDefaultAsync(Expression<Func<Image, bool>> predicate);
    }
}