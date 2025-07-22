using MediatR;
using Microsoft.AspNetCore.Http;

namespace TravelEase.Application.ImageManagement.ForCityEntity.Commands
{
    public record UploadCityImageCommand : IRequest
    {
        public Guid CityId { get; init; }
        public IFormFile File { get; init; }
    }
}