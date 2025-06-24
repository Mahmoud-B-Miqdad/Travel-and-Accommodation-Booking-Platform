namespace TravelEase.Domain.Aggregates.City
{
    public class City
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string CountryName { get; set; }
        public string CountryCode { get; set; }
        public string PostOffice { get; set; }
    }
}