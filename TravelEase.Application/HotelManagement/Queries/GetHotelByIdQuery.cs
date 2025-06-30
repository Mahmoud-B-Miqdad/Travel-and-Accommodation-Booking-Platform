using MediatR;
using TravelEase.Application.HotelManagement.DTOs.Responses;

namespace TravelEase.Application.HotelManagement.Queries
{
    public record GetHotelByIdQuery : IRequest<HotelWithoutRoomsResponse?>
    {
        public Guid Id { get; set; }
    }
}