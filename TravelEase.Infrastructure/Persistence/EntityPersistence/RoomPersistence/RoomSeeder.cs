using TravelEase.Domain.Aggregates.Rooms;

namespace TravelEase.Infrastructure.Persistence.EntityPersistence.RoomPersistence
{
    public static class RoomSeeder
    {
        public static List<Room> GetSeedData()
        {
            return new List<Room>
            {
                new Room
                {
                    Id = Guid.Parse("b03c67e0-3c3c-4a24-9fa0-9632d693ab01"),
                    RoomTypeId = Guid.Parse("0af3d54f-214f-4c33-8a9e-2389329e0001"),
                    AdultsCapacity = 2,
                    ChildrenCapacity = 1,
                    View = "Sea View",
                    Rating = 4.5f
                },
                new Room
                {
                    Id = Guid.Parse("1d2cbcb0-6727-4d3e-8c90-1c7c7e48f482"),
                    RoomTypeId = Guid.Parse("0fa4010a-8c8a-4d8d-a6e7-3927c40a0002"), 
                    AdultsCapacity = 3,
                    ChildrenCapacity = 2,
                    View = "Mountain View",
                    Rating = 4.2f
                },
                new Room
                {
                    Id = Guid.Parse("e47fcdf4-6355-4ea3-a33f-59ff56ad1f03"),
                    RoomTypeId = Guid.Parse("a2b8e135-84ef-4c34-83b1-1f68240b0003"), 
                    AdultsCapacity = 1,
                    ChildrenCapacity = 0,
                    View = "City View",
                    Rating = 3.8f
                },
                new Room
                {
                    Id = Guid.Parse("99d8eb70-2190-4238-9f00-22f6e5b5a505"),
                    RoomTypeId = Guid.Parse("b4fca5e3-2e10-4b85-9f98-0b16d50c0004"), 
                    AdultsCapacity = 2,
                    ChildrenCapacity = 2,
                    View = "Pool View",
                    Rating = 4.9f
                },
                new Room
                {
                    Id = Guid.Parse("10cdbbe9-1e91-4dc5-94e5-cfb6fce5c607"),
                    RoomTypeId = Guid.Parse("cb0f3c22-7a44-44a7-9e43-7cc3e70d0005"), 
                    AdultsCapacity = 4,
                    ChildrenCapacity = 2,
                    View = "Garden View",
                    Rating = 4.0f
                }
            };
        }
    }
}