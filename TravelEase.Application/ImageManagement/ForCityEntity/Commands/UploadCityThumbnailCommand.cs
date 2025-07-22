using MediatR;
using Microsoft.AspNetCore.Http;

namespace TravelEase.Application.ImageManagement.ForCityEntity.Commands
{
    public record UploadCityThumbnailCommand : IRequest
    {
        public Guid CityId { get; init; }
        public IFormFile File { get; init; }
    }
}