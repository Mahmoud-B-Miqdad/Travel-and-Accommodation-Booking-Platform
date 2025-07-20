using TravelEase.Domain.Common.Models.ImageModels;
using TravelEase.Domain.Common.Models.PaginationModels;

namespace TravelEase.Domain.Common.Interfaces
{
    public interface IImageService
    {
        public Task<PaginatedList<string>> GetAllImagesAsync
            (Guid entityId, int pageNumber, int pageSize);
        public Task UploadImageAsync(ImageCreationDTO imageCreationDto);
        public Task UploadThumbnailAsync(ImageCreationDTO imageCreationDto);
        public Task DeleteImageAsync(Guid entityId, Guid imageId);
    }
}