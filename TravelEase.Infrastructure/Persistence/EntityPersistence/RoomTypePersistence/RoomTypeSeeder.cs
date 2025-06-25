using TravelEase.Domain.Aggregates.Discounts;
using TravelEase.Domain.Aggregates.RoomAmenities;
using TravelEase.Domain.Aggregates.RoomTypes;
using TravelEase.Domain.Enums;

namespace TravelEase.Infrastructure.Persistence.EntityPersistence.RoomTypePersistence
{
    public static class RoomTypeSeeder
    {
        public static List<RoomType> GetSeedData()
        {
            return new List<RoomType>
            {
                new RoomType
                {
                    Id = Guid.Parse("rt000001-0000-0000-0000-000000000001"),
                    HotelId = Guid.Parse("a1111111-aaaa-4aaa-aaaa-aaaaaaaaaaaa"), 
                    Category = RoomCategory.Single,
                    PricePerNight = 120.0f,
                },
                new RoomType
                {
                    Id = Guid.Parse("rt000002-0000-0000-0000-000000000002"),
                    HotelId = Guid.Parse("a1111111-aaaa-4aaa-aaaa-aaaaaaaaaaaa"),
                    Category = RoomCategory.Double,
                    PricePerNight = 200.0f,
                },
                new RoomType
                {
                    Id = Guid.Parse("rt000003-0000-0000-0000-000000000003"),
                    HotelId = Guid.Parse("a2222222-bbbb-4bbb-bbbb-bbbbbbbbbbbb"), 
                    Category = RoomCategory.Double,
                    PricePerNight = 180.0f,
                },
                new RoomType
                {
                    Id = Guid.Parse("rt000004-0000-0000-0000-000000000004"),
                    HotelId = Guid.Parse("a3333333-cccc-4ccc-cccc-cccccccccccc"), 
                    Category = RoomCategory.Suite,
                    PricePerNight = 350.0f,
                },
                new RoomType
                {
                    Id = Guid.Parse("rt000005-0000-0000-0000-000000000005"),
                    HotelId = Guid.Parse("a4444444-dddd-4ddd-dddd-dddddddddddd"), 
                    Category = RoomCategory.Double,
                    PricePerNight = 220.0f,
                },
                new RoomType
                {
                    Id = Guid.Parse("rt000006-0000-0000-0000-000000000006"),
                    HotelId = Guid.Parse("a5555555-eeee-4eee-eeee-eeeeeeeeeeee"), 
                    Category = RoomCategory.Single,
                    PricePerNight = 110.0f,
                },
                new RoomType
                {
                    Id = Guid.Parse("rt000007-0000-0000-0000-000000000007"),
                    HotelId = Guid.Parse("a5555555-eeee-4eee-eeee-eeeeeeeeeeee"),
                    Category = RoomCategory.Suite,
                    PricePerNight = 380.0f,
                },
                new RoomType
                {
                    Id = Guid.Parse("rt000008-0000-0000-0000-000000000008"),
                    HotelId = Guid.Parse("a3333333-cccc-4ccc-cccc-cccccccccccc"),
                    Category = RoomCategory.Single,
                    PricePerNight = 130.0f,
                },
                new RoomType
                {
                    Id = Guid.Parse("rt000009-0000-0000-0000-000000000009"),
                    HotelId = Guid.Parse("a2222222-bbbb-4bbb-bbbb-bbbbbbbbbbbb"),
                    Category = RoomCategory.Suite,
                    PricePerNight = 400.0f,
                },
                new RoomType
                {
                    Id = Guid.Parse("rt000010-0000-0000-0000-000000000010"),
                    HotelId = Guid.Parse("a4444444-dddd-4ddd-dddd-dddddddddddd"),
                    Category = RoomCategory.Double,
                    PricePerNight = 210.0f,
                }
            };
        }
    }
}
