using TravelEase.Domain.Aggregates.Hotels;

namespace TravelEase.Infrastructure.Persistence.EntityPersistence.HotelPersistence
{
    public static class HotelSeeder
    {
        public static List<Hotel> GetSeedData()
        {
            return new List<Hotel>
            {
                new Hotel
                {
                    Id = Guid.Parse("a1111111-aaaa-4aaa-aaaa-aaaaaaaaaaaa"),
                    CityId = Guid.Parse("c1f1d9e4-6a51-4a0a-9b6b-111111111111"), 
                    Name = "Jerusalem Grand Hotel",
                    Rating = 4.7f,
                    StreetAddress = "123 Old City St.",
                    Description = "A luxury hotel in the heart of Jerusalem.",
                    PhoneNumber = "+970212345678",
                    FloorsNumber = 10,
                    OwnerName = "John Doe"
                },
                new Hotel
                {
                    Id = Guid.Parse("a2222222-bbbb-4bbb-bbbb-bbbbbbbbbbbb"),
                    CityId = Guid.Parse("c1f1d9e4-6a51-4a0a-9b6b-111111111111"), 
                    Name = "Jerusalem Boutique Inn",
                    Rating = 4.3f,
                    StreetAddress = "45 King David Blvd.",
                    Description = "Cozy boutique hotel near major landmarks.",
                    PhoneNumber = "+970212345679",
                    FloorsNumber = 5,
                    OwnerName = "Sarah Cohen"
                },
                new Hotel
                {
                    Id = Guid.Parse("a3333333-cccc-4ccc-cccc-cccccccccccc"),
                    CityId = Guid.Parse("c1f1d9e4-6a51-4a0a-9b6b-222222222222"),
                    Name = "Amman Royal Hotel",
                    Rating = 4.5f,
                    StreetAddress = "12 Rainbow Street",
                    Description = "Elegant hotel with modern amenities.",
                    PhoneNumber = "+96265123456",
                    FloorsNumber = 8,
                    OwnerName = "Omar Al-Khatib"
                },
                new Hotel
                {
                    Id = Guid.Parse("a4444444-dddd-4ddd-dddd-dddddddddddd"),
                    CityId = Guid.Parse("c1f1d9e4-6a51-4a0a-9b6b-333333333333"), 
                    Name = "Cairo Nile View",
                    Rating = 4.2f,
                    StreetAddress = "Nile Corniche",
                    Description = "Hotel with beautiful Nile river views.",
                    PhoneNumber = "+20212345678",
                    FloorsNumber = 12,
                    OwnerName = "Fatima Hassan"
                },
                new Hotel
                {
                    Id = Guid.Parse("a5555555-eeee-4eee-eeee-eeeeeeeeeeee"),
                    CityId = Guid.Parse("c1f1d9e4-6a51-4a0a-9b6b-222222222222"),
                    Name = "Amman City Center Hotel",
                    Rating = 4.1f,
                    StreetAddress = "3 Downtown Rd.",
                    Description = "Conveniently located hotel in downtown Amman.",
                    PhoneNumber = "+96265123457",
                    FloorsNumber = 7,
                    OwnerName = "Leila Mansour"
                }
            };
        }
    }
}