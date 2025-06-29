namespace TravelEase.Application.CityManagement.DTOs.Requests
{
    public class CityForUpdateRequest
    {
        public string Name { get; set; }
        public string CountryName { get; set; }
        public string CountryCode { get; set; }
        public string PostOffice { get; set; }
    }
}