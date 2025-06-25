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
                    Id = new Guid("0af3d54f-214f-4c33-8a9e-2389329e0001"),
                    HotelId = new Guid("9d6d68f6-c6b0-41a9-b6f1-72d7d340b101"), 
                    Category = RoomCategory.Single,
                    PricePerNight = 120.0f,
                },
                new RoomType
                {
                    Id = new Guid("0fa4010a-8c8a-4d8d-a6e7-3927c40a0002"),
                    HotelId = new Guid("9d6d68f6-c6b0-41a9-b6f1-72d7d340b101"),
                    Category = RoomCategory.Double,
                    PricePerNight = 200.0f,
                },
                new RoomType
                {
                    Id = new Guid("a2b8e135-84ef-4c34-83b1-1f68240b0003"),
                    HotelId = new Guid("04a5ffeb-9134-4097-9b4e-d4783c194102"), 
                    Category = RoomCategory.Double,
                    PricePerNight = 180.0f,
                },
                new RoomType
                {
                    Id = new Guid("b4fca5e3-2e10-4b85-9f98-0b16d50c0004"),
                    HotelId = new Guid("69702fd3-fb46-4694-b5fc-3de9c8b5a103"), 
                    Category = RoomCategory.Suite,
                    PricePerNight = 350.0f,
                },
                new RoomType
                {
                    Id = new Guid("cb0f3c22-7a44-44a7-9e43-7cc3e70d0005"),
                    HotelId = new Guid("b3dd1dce-e79f-4d00-b5e3-f2d4d0a4f104"),
                    Category = RoomCategory.Double,
                    PricePerNight = 220.0f,
                },
                new RoomType
                {
                    Id = new Guid("de2d1f5a-6237-4e49-8b10-24de1e0e0006"),
                    HotelId = new Guid("fbfc9c27-799b-4d7e-b12a-dbbdc914f105"), 
                    Category = RoomCategory.Single,
                    PricePerNight = 110.0f,
                },
                new RoomType
                {
                    Id = new Guid("fbc8e6f6-0fca-4a34-8f9d-41b37a0f0007"),
                    HotelId = new Guid("fbfc9c27-799b-4d7e-b12a-dbbdc914f105"),
                    Category = RoomCategory.Suite,
                    PricePerNight = 380.0f,
                },
                new RoomType
                {
                    Id = new Guid("0e9a34a0-cf9e-4a41-b27c-96c3b5100008"),
                    HotelId = new Guid("69702fd3-fb46-4694-b5fc-3de9c8b5a103"),
                    Category = RoomCategory.Single,
                    PricePerNight = 130.0f,
                },
                new RoomType
                {
                    Id = new Guid("1a734e0b-78c3-401e-b8b4-32d129110009"),
                    HotelId = new Guid("04a5ffeb-9134-4097-9b4e-d4783c194102"),
                    Category = RoomCategory.Suite,
                    PricePerNight = 400.0f,
                },
                new RoomType
                {
                    Id = new Guid("2690e45c-015f-4c58-8a33-0db4f3120010"),
                    HotelId = new Guid("b3dd1dce-e79f-4d00-b5e3-f2d4d0a4f104"),
                    Category = RoomCategory.Double,
                    PricePerNight = 210.0f,
                }
            };
        }
    }
}