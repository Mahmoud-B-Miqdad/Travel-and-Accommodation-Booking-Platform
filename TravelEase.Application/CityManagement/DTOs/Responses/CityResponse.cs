using TravelEase.Application.HotelManagement.DTOs.Responses;

namespace TravelEase.Application.CityManagement.DTOs.Responses
{
    public class CityResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string CountryName { get; set; }
        public string CountryCode { get; set; }
        public string PostOffice { get; set; }
        public IList<HotelWithoutRoomsResponse> Hotels { get; set; }
    }
}