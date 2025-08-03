namespace TravelEase.Application.HotelManagement.DTOs.Responses
{
    public class HotelWithoutRoomsResponse
    {
        public Guid Id { get; init; }
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