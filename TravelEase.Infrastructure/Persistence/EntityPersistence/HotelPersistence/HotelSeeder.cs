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
                    Id = new Guid("9d6d68f6-c6b0-41a9-b6f1-72d7d340b101"), 
                    CityId = new Guid("b6c9cf0e-31a6-4a35-a932-05f1e58f4a01"),
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
                    Id = new Guid("04a5ffeb-9134-4097-9b4e-d4783c194102"), 
                    CityId = new Guid("b6c9cf0e-31a6-4a35-a932-05f1e58f4a01"),
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
                    Id = new Guid("69702fd3-fb46-4694-b5fc-3de9c8b5a103"), 
                    CityId = new Guid("f5f7c2b4-70a1-4b99-b6f2-8e416ab2de02"),
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
                    Id = new Guid("b3dd1dce-e79f-4d00-b5e3-f2d4d0a4f104"), 
                    CityId = new Guid("d3be3a21-8eac-48fa-a5f0-6c9e3c53ee03"),
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
                    Id = new Guid("fbfc9c27-799b-4d7e-b12a-dbbdc914f105"), 
                    CityId = new Guid("f5f7c2b4-70a1-4b99-b6f2-8e416ab2de02"),
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