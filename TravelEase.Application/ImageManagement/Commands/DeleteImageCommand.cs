using MediatR;

namespace TravelEase.Application.ImageManagement.Commands
{
    public class DeleteImageCommand : IRequest
    {
        public Guid HotelId { get; set; }
        public Guid ImageId { get; set; }
    }
}