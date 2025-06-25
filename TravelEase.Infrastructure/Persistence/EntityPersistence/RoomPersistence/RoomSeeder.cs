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
                    RoomTypeId = Guid.Parse("6aa6c182-5f0c-4872-bc1e-133d6931c101"),
                    AdultsCapacity = 2,
                    ChildrenCapacity = 1,
                    View = "Sea View",
                    Rating = 4.5f
                },
                new Room
                {
                    Id = Guid.Parse("1d2cbcb0-6727-4d3e-8c90-1c7c7e48f482"),
                    RoomTypeId = Guid.Parse("6aa6c182-5f0c-4872-bc1e-133d6931c102"),
                    AdultsCapacity = 3,
                    ChildrenCapacity = 2,
                    View = "Mountain View",
                    Rating = 4.2f
                },
                new Room
                {
                    Id = Guid.Parse("e47fcdf4-6355-4ea3-a33f-59ff56ad1f03"),
                    RoomTypeId = Guid.Parse("6aa6c182-5f0c-4872-bc1e-133d6931c103"),
                    AdultsCapacity = 1,
                    ChildrenCapacity = 0,
                    View = "City View",
                    Rating = 3.8f
                },
                new Room
                {
                    Id = Guid.Parse("99d8eb70-2190-4238-9f00-22f6e5b5a505"),
                    RoomTypeId = Guid.Parse("6aa6c182-5f0c-4872-bc1e-133d6931c104"),
                    AdultsCapacity = 2,
                    ChildrenCapacity = 2,
                    View = "Pool View",
                    Rating = 4.9f
                },
                new Room
                {
                    Id = Guid.Parse("10cdbbe9-1e91-4dc5-94e5-cfb6fce5c607"),
                    RoomTypeId = Guid.Parse("6aa6c182-5f0c-4872-bc1e-133d6931c105"),
                    AdultsCapacity = 4,
                    ChildrenCapacity = 2,
                    View = "Garden View",
                    Rating = 4.0f
                }
            };
        }
    }
}