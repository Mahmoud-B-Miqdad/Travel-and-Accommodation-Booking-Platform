namespace TravelEase.Application.CityManagement.DTOs.Requests
{
    public class CityForCreationRequest
    {

        public string Name { get; set; } = string.Empty;
        public string CountryName { get; set; } = string.Empty;
        public string CountryCode { get; set; } = string.Empty;
        public string PostOffice { get; set; } = string.Empty;
    }
}