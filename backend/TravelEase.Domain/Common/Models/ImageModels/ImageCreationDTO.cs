using TravelEase.Domain.Enums;

namespace TravelEase.Domain.Common.Models.ImageModels
{
    public record ImageCreationDTO
    {
        public Guid EntityId { get; set; }
        public string Base64Content { get; set; }
        public ImageFormat Format { get; set; }
        public ImageType Type { get; set; }
    }
}