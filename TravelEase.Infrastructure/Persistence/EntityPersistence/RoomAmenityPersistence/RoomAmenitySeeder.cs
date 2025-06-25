using TravelEase.Domain.Aggregates.RoomAmenities;

namespace TravelEase.Infrastructure.Persistence.EntityPersistence.RoomAmenityPersistence
{
    public static class RoomAmenitySeeder
    {
        public static List<RoomAmenity> GetSeedData()
        {
            return new List<RoomAmenity>
            {
                new RoomAmenity
                {
                    Id = Guid.Parse("amenity0001-0000-0000-0000-000000000001"),
                    Name = "Free WiFi",
                    Description = "High-speed wireless internet access available throughout the room."
                },
                new RoomAmenity
                {
                    Id = Guid.Parse("amenity0002-0000-0000-0000-000000000002"),
                    Name = "Air Conditioning",
                    Description = "Individual air conditioning unit for personalized comfort."
                },
                new RoomAmenity
                {
                    Id = Guid.Parse("amenity0003-0000-0000-0000-000000000003"),
                    Name = "Flat Screen TV",
                    Description = "42-inch flat screen television with cable channels."
                },
                new RoomAmenity
                {
                    Id = Guid.Parse("amenity0004-0000-0000-0000-000000000004"),
                    Name = "Daily Housekeeping",
                    Description = "Daily cleaning service to keep your room tidy."
                },
                new RoomAmenity
                {
                    Id = Guid.Parse("amenity0005-0000-0000-0000-000000000005"),
                    Name = "Parking",
                    Description = "Free parking space available for guests."
                },
            };
        }
    }
}