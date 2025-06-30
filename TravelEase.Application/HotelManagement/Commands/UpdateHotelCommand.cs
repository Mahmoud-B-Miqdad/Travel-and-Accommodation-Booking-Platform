using MediatR;

namespace TravelEase.Application.HotelManagement.Commands
{
    public class UpdateHotelCommand : IRequest
    {
        public Guid Id { get; set; }
        public Guid CityId { get; set; }
        public string OwnerName { get; set; }
        public string Name { get; set; }
        public float Rating { get; set; }
        public string StreetAddress { get; set; }
        public string Description { get; set; }
        public string PhoneNumber { get; set; }
        public int FloorsNumber { get; set; }
    }
}