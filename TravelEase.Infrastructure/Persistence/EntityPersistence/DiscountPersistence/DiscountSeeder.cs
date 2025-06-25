using TravelEase.Domain.Aggregates.Discounts;

namespace TravelEase.Infrastructure.Persistence.EntityPersistence.DiscountPersistence
{
    public static class DiscountSeeder
    {
        public static List<Discount> GetSeedData()
        {
            return new List<Discount>
            {
                new Discount
                {
                    Id = Guid.Parse("disc0001-0000-0000-0000-000000000001"),
                    RoomTypeId = Guid.Parse("rt000001-0000-0000-0000-000000000001"), 
                    DiscountPercentage = 10f,
                    FromDate = new DateTime(2025, 7, 1),
                    ToDate = new DateTime(2025, 7, 31)
                },
                new Discount
                {
                    Id = Guid.Parse("disc0002-0000-0000-0000-000000000002"),
                    RoomTypeId = Guid.Parse("rt000004-0000-0000-0000-000000000004"), 
                    DiscountPercentage = 15f,
                    FromDate = new DateTime(2025, 8, 1),
                    ToDate = new DateTime(2025, 8, 15)
                },
                new Discount
                {
                    Id = Guid.Parse("disc0003-0000-0000-0000-000000000003"),
                    RoomTypeId = Guid.Parse("rt000005-0000-0000-0000-000000000005"), 
                    DiscountPercentage = 5f,
                    FromDate = new DateTime(2025, 9, 1),
                    ToDate = new DateTime(2025, 9, 10)
                },
                new Discount
                {
                    Id = Guid.Parse("disc0004-0000-0000-0000-000000000004"),
                    RoomTypeId = Guid.Parse("rt000007-0000-0000-0000-000000000007"), 
                    DiscountPercentage = 20f,
                    FromDate = new DateTime(2025, 12, 20),
                    ToDate = new DateTime(2025, 12, 31)
                },
                new Discount
                {
                    Id = Guid.Parse("disc0005-0000-0000-0000-000000000005"),
                    RoomTypeId = Guid.Parse("rt000009-0000-0000-0000-000000000009"), 
                    DiscountPercentage = 12.5f,
                    FromDate = new DateTime(2025, 10, 1),
                    ToDate = new DateTime(2025, 10, 30)
                }
            };
        }
    }
}