using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;
using TravelEase.Domain.Aggregates.Images;
using TravelEase.Domain.Common.Interfaces;
using TravelEase.Domain.Common.Models.ImageModels;
using TravelEase.Domain.Common.Models.PaginationModels;
using TravelEase.Domain.Common.Models.SettingModels;
using TravelEase.Domain.Enums;
using TravelEase.Domain.Exceptions;

namespace TravelEase.Infrastructure.Persistence.Services.ImageServices
{
    public class CloudinaryImageService : IImageService
    {
        private readonly Cloudinary _cloudinary;
        private readonly IImageRepository _imageRepository;
        private readonly ILogger<CloudinaryImageService> _logger;

        public CloudinaryImageService(
            IOptions<CloudinarySettings> settings,
            IImageRepository imageRepository,
            ILogger<CloudinaryImageService> logger)
        {
            var account = new Account(
            settings.Value.CloudName,
            settings.Value.ApiKey,
                settings.Value.ApiSecret);

            _cloudinary = new Cloudinary(account);
            _imageRepository = imageRepository;
            _logger = logger;
        }

        public async Task<PaginatedList<string>> GetAllImagesAsync(Guid entityId, int pageNumber, int pageSize)
        {
            return await _imageRepository.GetAllImageUrlsByEntityIdAsync(entityId, pageNumber, pageSize);
        }

        public async Task UploadImageAsync(ImageCreationDTO imageCreationDto)
        {
            var imageId = Guid.NewGuid();
            var uploadResult = await UploadToCloudinaryAsync(imageCreationDto, imageId);

            var image = new Image
            {
                Id = imageId,
                Url = uploadResult.SecureUrl.ToString(),
                EntityId = imageCreationDto.EntityId,
                Format = imageCreationDto.Format,
                Type = imageCreationDto.Type
            };

            await _imageRepository.AddAsync(image);
            _logger.LogInformation("Image uploaded successfully to Cloudinary.");
        }

        public async Task UploadThumbnailAsync(ImageCreationDTO imageCreationDto)
        {
            var existingThumbnail = await _imageRepository
                .GetSingleOrDefaultAsync(i =>
                    i.Type == ImageType.Thumbnail &&
                    i.EntityId == imageCreationDto.EntityId);

            if (existingThumbnail is not null)
            {
                await DeleteImageFromCloudinaryAsync(existingThumbnail.Id, existingThumbnail.Format);
                await ReplaceImageAsync(existingThumbnail, imageCreationDto);
            }
            else
            {
                await UploadImageAsync(imageCreationDto);
            }
        }

        public async Task DeleteImageAsync(Guid entityId, Guid imageId)
        {
            var image = await _imageRepository
                .GetSingleOrDefaultAsync(i => i.Id == imageId && i.EntityId == entityId);

            if (image is null)
                throw new NotFoundException($"Image with ID {imageId} not found for Entity {entityId}");

            await DeleteImageFromCloudinaryAsync(image.Id, image.Format);

            _imageRepository.Remove(image);

            _logger.LogInformation($"Image with ID {image.Id} deleted from DB and Cloudinary.");
        }

        private async Task ReplaceImageAsync(Image existingImage, ImageCreationDTO dto)
        {
            var uploadResult = await UploadToCloudinaryAsync(dto, existingImage.Id);

            existingImage.Url = uploadResult.SecureUrl.ToString();
            existingImage.Format = dto.Format;

            _imageRepository.Update(existingImage);
            _logger.LogInformation("Thumbnail replaced and updated successfully in Cloudinary.");
        }

        private async Task<ImageUploadResult> UploadToCloudinaryAsync(ImageCreationDTO dto, Guid imageId)
        {
            var imageBytes = Convert.FromBase64String(dto.Base64Content);
            var format = dto.Format.ToString().ToLower();

            using var stream = new MemoryStream(imageBytes);

            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription($"{imageId}.{format}", stream),
                PublicId = imageId.ToString(),
                Overwrite = true,
                Folder = "images"
            };

            var result = await _cloudinary.UploadAsync(uploadParams);

            if (result.StatusCode != HttpStatusCode.OK)
                throw new InvalidOperationException("Image upload to Cloudinary failed.");

            return result;
        }

        private async Task DeleteImageFromCloudinaryAsync(Guid imageId, ImageFormat format)
        {
            var publicId = $"{imageId}.{format.ToString().ToLower()}";

            var deletionParams = new DeletionParams(publicId)
            {
                ResourceType = ResourceType.Image
            };

            var result = await _cloudinary.DestroyAsync(deletionParams);

            if (result.Result != "ok" && result.Result != "not found")
                throw new InvalidOperationException($"Error deleting image with ID {imageId}");

            _logger.LogInformation($"Image with ID {imageId} deleted from Cloudinary.");
        }
    }
}