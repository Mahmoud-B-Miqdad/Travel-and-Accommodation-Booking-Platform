using TravelEase.Domain.Aggregates.Hotels;

namespace TravelEase.Domain.Aggregates.Cities
{
    public class City
    {
        public Guid Id { get; set; }
        public List<Hotel> Hotels { get; set; }
        public string Name { get; set; }
        public string CountryName { get; set; }
        public string CountryCode { get; set; }
        public string PostOffice { get; set; }
    }
}