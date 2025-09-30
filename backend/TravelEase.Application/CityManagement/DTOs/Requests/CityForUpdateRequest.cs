namespace TravelEase.Application.CityManagement.DTOs.Requests
{
    public class CityForUpdateRequest
    {
        public string Name { get; init; }
        public string CountryName { get; init; }
        public string CountryCode { get; init; }
        public string PostOffice { get; init; }
    }
}