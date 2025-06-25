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
                    Id = new Guid("b48fa00c-4f6d-4c91-a973-4cb94c80ec01"),
                    Name = "Free WiFi",
                    Description = "High-speed wireless internet access available throughout the room."
                },
                new RoomAmenity
                {
                    Id = new Guid("f79904be-34aa-41ec-a580-40989e3b3602"),
                    Name = "Air Conditioning",
                    Description = "Individual air conditioning unit for personalized comfort."
                },
                new RoomAmenity
                {
                    Id = new Guid("a11d00f2-4fc6-45f1-bc7a-3de3cc548603"),
                    Name = "Flat Screen TV",
                    Description = "42-inch flat screen television with cable channels."
                },
                new RoomAmenity
                {
                    Id = new Guid("de1c3e3d-85f1-4c4e-9ec8-34de25173a04"),
                    Name = "Daily Housekeeping",
                    Description = "Daily cleaning service to keep your room tidy."
                },
                new RoomAmenity
                {
                    Id = new Guid("e7c8c5ae-4411-4d57-ae1e-0e43a6cc6f05"),
                    Name = "Parking",
                    Description = "Free parking space available for guests."
                },
            };
        }
    }
}