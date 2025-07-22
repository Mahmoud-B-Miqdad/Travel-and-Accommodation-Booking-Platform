using MediatR;
using Microsoft.AspNetCore.Http;

namespace TravelEase.Application.ImageManagement.ForHotelEntity.Commands
{
    public record UploadHotelImageCommand : IRequest
    {
        public Guid HotelId { get; init; }
        public IFormFile File { get; init; }
    }
}