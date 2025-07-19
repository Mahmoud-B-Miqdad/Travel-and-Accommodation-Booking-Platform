using TravelEase.Domain.Common.Interfaces;

namespace TravelEase.Domain.Aggregates.Images
{
    public interface IImageRepository : ICrudRepository<Image>
    {
        Task<List<string>> GetAllImageUrlsByEntityIdAsync(Guid entityId);
    }
}