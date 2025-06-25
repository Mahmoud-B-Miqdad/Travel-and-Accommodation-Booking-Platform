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
                    Id = Guid.Parse("de1c4d04-0c38-4d08-b0b7-0a1a16be1a11"),
                    RoomId = Guid.Parse("b03c67e0-3c3c-4a24-9fa0-9632d693ab01"),
                    UserId = Guid.Parse("8a16c3b4-d0d6-4d13-9582-55c0a1a2f301"),
                    CheckInDate = new DateTime(2025, 7, 10),
                    CheckOutDate = new DateTime(2025, 7, 15),
                    BookingDate = DateTime.UtcNow,
                    Price = 600.00
                },
                new Booking
                {
                    Id = Guid.Parse("b1f25d35-b8ef-478c-90a6-4a6fdf674a45"),
                    RoomId = Guid.Parse("1d2cbcb0-6727-4d3e-8c90-1c7c7e48f482"),
                    UserId = Guid.Parse("f1f6b53f-2e4d-4b7a-9d7e-e9c8a0eb1b02"),
                    CheckInDate = new DateTime(2025, 8, 1),
                    CheckOutDate = new DateTime(2025, 8, 6),
                    BookingDate = DateTime.UtcNow,
                    Price = 800.00
                },
                new Booking
                {
                    Id = Guid.Parse("f3d0d7c9-c18b-45f6-86e3-f6b95e3f8cb7"),
                    RoomId = Guid.Parse("e47fcdf4-6355-4ea3-a33f-59ff56ad1f03"),
                    UserId = Guid.Parse("112c830f-7fc3-4b26-b6ef-80b3be70b003"),
                    CheckInDate = new DateTime(2025, 9, 5),
                    CheckOutDate = new DateTime(2025, 9, 10),
                    BookingDate = DateTime.UtcNow,
                    Price = 700.00
                },
                new Booking
                {
                    Id = Guid.Parse("a0c5c142-1e94-4a5c-9241-32f4fbb1b004"),
                    RoomId = Guid.Parse("99d8eb70-2190-4238-9f00-22f6e5b5a505"),
                    UserId = Guid.Parse("a43826b5-ffbb-4c5d-9a45-3a8f12d8d005"),
                    CheckInDate = new DateTime(2025, 10, 1),
                    CheckOutDate = new DateTime(2025, 10, 5),
                    BookingDate = DateTime.UtcNow,
                    Price = 650.00
                },
                new Booking
                {
                    Id = Guid.Parse("d248dd73-98a2-45e2-8e4e-6eb8b78cc006"),
                    RoomId = Guid.Parse("10cdbbe9-1e91-4dc5-94e5-cfb6fce5c607"),
                    UserId = Guid.Parse("eb4a71b6-0f3e-4aa5-917b-99696e42f907"),
                    CheckInDate = new DateTime(2025, 11, 12),
                    CheckOutDate = new DateTime(2025, 11, 15),
                    BookingDate = DateTime.UtcNow,
                    Price = 500.00
                }
            };
        }
    }
}