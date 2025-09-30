using MediatR;
using TravelEase.Application.HotelManagement.DTOs.Responses;

namespace TravelEase.Application.HotelManagement.Commands
{
    public class CreateHotelCommand : IRequest<HotelWithoutRoomsResponse?>
    {
        public Guid CityId { get; init; }
        public string OwnerName { get; init; }
        public string Name { get; init; }
        public float Rating { get; init; }
        public string StreetAddress { get; init; }
        public string Description { get; init; }
        public string PhoneNumber { get; init; }
        public int FloorsNumber { get; init; }
    }
}