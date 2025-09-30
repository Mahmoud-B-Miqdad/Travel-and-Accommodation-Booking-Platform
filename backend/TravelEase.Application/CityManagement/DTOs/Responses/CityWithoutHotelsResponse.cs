namespace TravelEase.Application.CityManagement.DTOs.Responses
{
    public class CityWithoutHotelsResponse
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public string CountryName { get; init; }
        public string CountryCode { get; init; } 
        public string PostOffice { get; init; }
    }
}