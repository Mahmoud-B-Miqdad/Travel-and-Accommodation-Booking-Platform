using TravelEase.Domain.Common.Models.ImageModels;

namespace TravelEase.Domain.Common.Interfaces
{
    public interface IImageService
    {
        public Task<List<string>> GetAllImagesAsync(Guid entityId);
        public Task UploadImageAsync(ImageCreationDTO imageCreationDto);
        public Task UploadThumbnailAsync(ImageCreationDTO imageCreationDto);
        public Task DeleteImageAsync(Guid entityId, Guid imageId);
    }
}