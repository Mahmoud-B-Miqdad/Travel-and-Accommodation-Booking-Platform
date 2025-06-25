using Microsoft.EntityFrameworkCore;
using TravelEase.Domain.Aggregates.Bookings;
using TravelEase.Domain.Aggregates.Cities;
using TravelEase.Domain.Aggregates.Hotels;
using TravelEase.Domain.Aggregates.Reviews;
using TravelEase.Domain.Aggregates.Rooms;
using TravelEase.Domain.Aggregates.RoomTypes;
using TravelEase.Infrastructure.Persistence.EntityPersistence.BookingPersistence;
using TravelEase.Infrastructure.Persistence.EntityPersistence.CityPersistence;
using TravelEase.Infrastructure.Persistence.EntityPersistence.DiscountPersistence;
using TravelEase.Infrastructure.Persistence.EntityPersistence.HotelPersistence;
using TravelEase.Infrastructure.Persistence.EntityPersistence.ReviewPersistence;
using TravelEase.Infrastructure.Persistence.EntityPersistence.RoomAmenityPersistence;
using TravelEase.Infrastructure.Persistence.EntityPersistence.RoomPersistence;
using TravelEase.Infrastructure.Persistence.EntityPersistence.RoomTypePersistence;

namespace TravelEase.Infrastructure.Common.Extensions
{
    public static class DataSeederExtension
    {
        public static void SeedTables(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Booking>().HasData(BookingSeeder.GetSeedData());
            modelBuilder.Entity<Room>().HasData(RoomSeeder.GetSeedData());
            modelBuilder.Entity<City>().HasData(CitySeeder.GetSeedData());
            modelBuilder.Entity<Hotel>().HasData(HotelSeeder.GetSeedData());
            modelBuilder.Entity<RoomType>().HasData(RoomTypeSeeder.GetSeedData());
            modelBuilder.Entity<RoomType>().HasData(RoomAmenitySeeder.GetSeedData());
            modelBuilder.Entity<RoomType>().HasData(DiscountSeeder.GetSeedData());
            modelBuilder.Entity<Review>().HasData(ReviewSeeder.GetSeedData());
        }
    }
}