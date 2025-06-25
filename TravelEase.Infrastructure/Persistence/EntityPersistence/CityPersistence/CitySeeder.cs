using TravelEase.Domain.Aggregates.Cities;

namespace TravelEase.Infrastructure.Persistence.EntityPersistence.CityPersistence
{
    public static class CitySeeder
    {
        public static List<City> GetSeedData()
        {
            return new List<City>
            {
                new City
                {
                    Id = new Guid("b6c9cf0e-31a6-4a35-a932-05f1e58f4a01"), 
                    Name = "Jerusalem",
                    CountryName = "Palestine",
                    CountryCode = "PS",
                    PostOffice = "JRS001"
                },
                new City
                {
                    Id = new Guid("f5f7c2b4-70a1-4b99-b6f2-8e416ab2de02"), 
                    Name = "Amman",
                    CountryName = "Jordan",
                    CountryCode = "JO",
                    PostOffice = "AMN002"
                },
                new City
                {
                    Id = new Guid("d3be3a21-8eac-48fa-a5f0-6c9e3c53ee03"),
                    Name = "Cairo",
                    CountryName = "Egypt",
                    CountryCode = "EG",
                    PostOffice = "CAI003"
                }
            };
        }
    }
}