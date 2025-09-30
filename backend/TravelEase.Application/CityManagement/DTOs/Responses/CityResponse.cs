using TravelEase.Application.HotelManagement.DTOs.Responses;

namespace TravelEase.Application.CityManagement.DTOs.Responses
{
    public class CityResponse
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public string CountryName { get; init; }
        public string CountryCode { get; init; }
        public string PostOffice { get; init; }
        public IList<HotelWithoutRoomsResponse> Hotels { get; init; }
    }
}