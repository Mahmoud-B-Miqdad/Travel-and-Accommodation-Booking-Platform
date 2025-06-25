using Microsoft.EntityFrameworkCore;
using TravelEase.Domain.Aggregates.Cities;
using TravelEase.Domain.Aggregates.Discounts;
using TravelEase.Domain.Aggregates.Hotels;
using TravelEase.Domain.Aggregates.RoomAmenities;
using TravelEase.Domain.Aggregates.Rooms;
using TravelEase.Domain.Aggregates.RoomTypes;
using TravelEase.Infrastructure.Persistence.EntityPersistence.CityPersistence;
using TravelEase.Infrastructure.Persistence.EntityPersistence.DiscountPersistence;
using TravelEase.Infrastructure.Persistence.EntityPersistence.HotelPersistence;
using TravelEase.Infrastructure.Persistence.EntityPersistence.RoomAmenityPersistence;
using TravelEase.Infrastructure.Persistence.EntityPersistence.RoomPersistence;
using TravelEase.Infrastructure.Persistence.EntityPersistence.RoomTypePersistence;

namespace TravelEase.Infrastructure.Common.Extensions
{
    public static class DataSeederExtension
    {
        public static void SeedTables(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Room>().HasData(RoomSeeder.GetSeedData());
            modelBuilder.Entity<City>().HasData(CitySeeder.GetSeedData());
            modelBuilder.Entity<Hotel>().HasData(HotelSeeder.GetSeedData());
            modelBuilder.Entity<RoomType>().HasData(RoomTypeSeeder.GetSeedData());
            modelBuilder.Entity<RoomAmenity>().HasData(RoomAmenitySeeder.GetSeedData());
            modelBuilder.Entity<Discount>().HasData(DiscountSeeder.GetSeedData());
        }
    }
}