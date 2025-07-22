using MediatR;
using Microsoft.AspNetCore.Http;

namespace TravelEase.Application.ImageManagement.Commands
{
    internal class UploadHotelThumbnailCommand : IRequest
    {
        public Guid HotelId { get; init; }
        public IFormFile File { get; init; }
    }
}