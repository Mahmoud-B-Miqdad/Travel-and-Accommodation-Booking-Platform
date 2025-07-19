using TravelEase.Domain.Enums;

namespace TravelEase.Application.ImageManagement.Responses
{
    public record ImageCreationResponse
    {
        public Guid EntityId { get; set; }
        public string Base64Content { get; set; }
        public ImageFormat Format { get; set; }
        public ImageType Type { get; set; }
    }
}