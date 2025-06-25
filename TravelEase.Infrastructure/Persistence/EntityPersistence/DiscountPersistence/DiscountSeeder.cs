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
                    Id = new Guid("342e5362-4a61-4d11-a7e7-6b01b47e0001"),
                    RoomTypeId = new Guid("0af3d54f-214f-4c33-8a9e-2389329e0001"),
                    DiscountPercentage = 10f,
                    FromDate = new DateTime(2025, 7, 1),
                    ToDate = new DateTime(2025, 7, 31)
                },
                new Discount
                {
                    Id = new Guid("cb51a724-e4ca-46e9-9e3d-d59f3f770002"),
                    RoomTypeId = new Guid("b4fca5e3-2e10-4b85-9f98-0b16d50c0004"), 
                    DiscountPercentage = 15f,
                    FromDate = new DateTime(2025, 8, 1),
                    ToDate = new DateTime(2025, 8, 15)
                },
                new Discount
                {
                    Id = new Guid("cfe7a21e-344d-4ae3-8a62-7cc89b710003"),
                    RoomTypeId = new Guid("cb0f3c22-7a44-44a7-9e43-7cc3e70d0005"), 
                    DiscountPercentage = 5f,
                    FromDate = new DateTime(2025, 9, 1),
                    ToDate = new DateTime(2025, 9, 10)
                },
                new Discount
                {
                    Id = new Guid("7851f29b-6d7a-4f7c-83ea-4f1b8c9c0004"),
                    RoomTypeId = new Guid("fbc8e6f6-0fca-4a34-8f9d-41b37a0f0007"),
                    DiscountPercentage = 20f,
                    FromDate = new DateTime(2025, 12, 20),
                    ToDate = new DateTime(2025, 12, 31)
                },
                new Discount
                {
                    Id = new Guid("246cf7e5-8c03-4cc2-bb6a-7592a15c0005"),
                    RoomTypeId = new Guid("1a734e0b-78c3-401e-b8b4-32d129110009"), 
                    DiscountPercentage = 12.5f,
                    FromDate = new DateTime(2025, 10, 1),
                    ToDate = new DateTime(2025, 10, 30)
                }
            };
        }
    }
}