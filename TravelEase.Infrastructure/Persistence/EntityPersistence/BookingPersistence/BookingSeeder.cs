using TravelEase.Domain.Aggregates.Bookings;

namespace TravelEase.Infrastructure.Persistence.EntityPersistence.BookingPersistence
{
    public static class BookingSeeder
    {
        public static List<Booking> GetSeedData()
        {
            return new List<Booking>
            {
                new Booking
                {
                    Id = new Guid("45d3d4b7-65ef-4a3e-a19a-14a3f28a0001"),
                    RoomId = new Guid("03c3c1f2-82a9-4a44-81c3-59c4dbbd0001"),  
                    UserId = new Guid("7a1b6456-4a92-4973-90e2-d13aee7f0001"), 
                    CheckInDate = new DateTime(2025, 7, 10),
                    CheckOutDate = new DateTime(2025, 7, 15),
                    BookingDate = DateTime.UtcNow,
                    Price = 600.00
                },
                new Booking
                {
                    Id = new Guid("50abf2f4-f134-4b2e-9e8c-37e2e3020002"),
                    RoomId = new Guid("82bcf3a9-726d-4a19-bb1a-0dcae3440002"),
                    UserId = new Guid("6f3e982d-54d9-4e25-bf5d-8eaf21ac0002"),
                    CheckInDate = new DateTime(2025, 8, 1),
                    CheckOutDate = new DateTime(2025, 8, 6),
                    BookingDate = DateTime.UtcNow,
                    Price = 800.00
                },
                new Booking
                {
                    Id = new Guid("6ed7464a-cb0e-4bf3-a03a-f7bb18e90003"),
                    RoomId = new Guid("90d8e7b2-8b8b-4c77-8d06-3bcbad460003"),
                    UserId = new Guid("3fa23748-0f9e-4b1f-9784-69ca2f0c0003"),
                    CheckInDate = new DateTime(2025, 9, 5),
                    CheckOutDate = new DateTime(2025, 9, 10),
                    BookingDate = DateTime.UtcNow,
                    Price = 700.00
                },
                new Booking
                {
                    Id = new Guid("d3a2d7f3-7c85-4cb7-9bc0-9e25f5a20004"),
                    RoomId = new Guid("7f2e5a4a-0d6f-41ac-b25d-85aa6ad20004"),
                    UserId = new Guid("9d8be2bc-c7e5-49d1-8b9f-55a7290f0004"),
                    CheckInDate = new DateTime(2025, 10, 1),
                    CheckOutDate = new DateTime(2025, 10, 5),
                    BookingDate = DateTime.UtcNow,
                    Price = 650.00
                },
                new Booking
                {
                    Id = new Guid("eac18319-63a2-4cae-bc3e-cad2a45f0005"),
                    RoomId = new Guid("37cc8479-68d4-4bdc-9f06-549bdc850005"),
                    UserId = new Guid("22cfc8e0-9886-4c6e-bdb1-3c6746210005"),
                    CheckInDate = new DateTime(2025, 11, 12),
                    CheckOutDate = new DateTime(2025, 11, 15),
                    BookingDate = DateTime.UtcNow,
                    Price = 500.00
                }
            };
        }
    }
}