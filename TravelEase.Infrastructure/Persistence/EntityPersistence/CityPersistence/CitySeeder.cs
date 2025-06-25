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
                    Id = Guid.Parse("c1f1d9e4-6a51-4a0a-9b6b-111111111111"),
                    Name = "Jerusalem",
                    CountryName = "Palestine",
                    CountryCode = "PS",
                    PostOffice = "JRS001"
                },
                new City
                {
                    Id = Guid.Parse("c1f1d9e4-6a51-4a0a-9b6b-222222222222"),
                    Name = "Amman",
                    CountryName = "Jordan",
                    CountryCode = "JO",
                    PostOffice = "AMN002"
                },
                new City
                {
                    Id = Guid.Parse("c1f1d9e4-6a51-4a0a-9b6b-333333333333"),
                    Name = "Cairo",
                    CountryName = "Egypt",
                    CountryCode = "EG",
                    PostOffice = "CAI003"
                }
            };
        }
    }
}