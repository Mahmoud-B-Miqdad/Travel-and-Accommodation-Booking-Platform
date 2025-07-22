using Microsoft.AspNetCore.Http;
using TravelEase.Domain.Common.Models.ImageModels;
using TravelEase.Domain.Enums;
using TravelEase.Domain.Exceptions;

namespace TravelEase.Application.ImageManagement.Mappings
{
    public static class ImageFormFileMapper
    {
        public static async Task<ImageCreationDTO> CreateFromFormFileAsync
            (Guid entityId, IFormFile file, ImageType type)
        {
            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            var base64Content = Convert.ToBase64String(memoryStream.ToArray());

            var imageFormat = GetImageFormat(file.ContentType);
            if (imageFormat == null)
                throw new UnsupportedImageFormatException(file.ContentType);

            return new ImageCreationDTO
            {
                EntityId = entityId,
                Base64Content = base64Content,
                Format = imageFormat.Value,
                Type = type
            };
        }

        private static ImageFormat? GetImageFormat(string contentType)
        {
            return contentType.ToLower() switch
            {
                "image/jpeg" => ImageFormat.Jpeg,
                "image/png" => ImageFormat.Png,
                _ => null
            };
        }
    }
}