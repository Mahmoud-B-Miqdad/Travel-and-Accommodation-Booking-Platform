using Microsoft.EntityFrameworkCore;
using TravelEase.Domain.Aggregates.Bookings;
using TravelEase.Domain.Aggregates.Cities;
using TravelEase.Domain.Aggregates.Discounts;
using TravelEase.Domain.Aggregates.Hotels;
using TravelEase.Domain.Aggregates.Reviews;
using TravelEase.Domain.Aggregates.RoomAmenities;
using TravelEase.Domain.Aggregates.Rooms;
using TravelEase.Domain.Aggregates.RoomTypes;
using TravelEase.Domain.Aggregates.Users;
using TravelEase.Infrastructure.Persistence.EntityPersistence.BookingPersistence;

namespace TravelEase.Infrastructure.Persistence
{
    public class TravelEaseDbContext : DbContext
    {
        public TravelEaseDbContext(DbContextOptions<TravelEaseDbContext> options) : base(options)
        {
        }

        public DbSet<Booking> Bookings { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<RoomType> RoomTypes { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<RoomAmenity> RoomAmenities { get; set; }
        public DbSet<Discount> Discounts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new BookingConfiguration());
        }
    }
}