using Microsoft.EntityFrameworkCore;
using TravelEase.Domain.Aggregates.Bookings;
using TravelEase.Domain.Aggregates.Cities;
using TravelEase.Domain.Aggregates.Discounts;
using TravelEase.Domain.Aggregates.Hotels;
using TravelEase.Domain.Aggregates.Images;
using TravelEase.Domain.Aggregates.Reviews;
using TravelEase.Domain.Aggregates.RoomAmenities;
using TravelEase.Domain.Aggregates.Rooms;
using TravelEase.Domain.Aggregates.RoomTypes;
using TravelEase.Domain.Aggregates.Users;
using TravelEase.Infrastructure.Common.Extensions;
using TravelEase.Infrastructure.Persistence.EntityPersistence.BookingPersistence;
using TravelEase.Infrastructure.Persistence.EntityPersistence.CityPersistence;
using TravelEase.Infrastructure.Persistence.EntityPersistence.DiscountPersistence;
using TravelEase.Infrastructure.Persistence.EntityPersistence.HotelPersistence;
using TravelEase.Infrastructure.Persistence.EntityPersistence.ImagePersistence;
using TravelEase.Infrastructure.Persistence.EntityPersistence.PaymentPersistence;
using TravelEase.Infrastructure.Persistence.EntityPersistence.ReviewPersistence;
using TravelEase.Infrastructure.Persistence.EntityPersistence.RoomAmenityPersistence;
using TravelEase.Infrastructure.Persistence.EntityPersistence.RoomPersistence;
using TravelEase.Infrastructure.Persistence.EntityPersistence.RoomTypePersistence;
using TravelEase.Infrastructure.Persistence.EntityPersistence.UserPersistence;

namespace TravelEase.Infrastructure.Persistence.Context
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
        public DbSet<Image> Images { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new BookingConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new RoomConfiguration());
            modelBuilder.ApplyConfiguration(new CityConfiguration());
            modelBuilder.ApplyConfiguration(new HotelConfiguration());
            modelBuilder.ApplyConfiguration(new ReviewConfiguration());
            modelBuilder.ApplyConfiguration(new RoomTypeConfiguration());
            modelBuilder.ApplyConfiguration(new DiscountConfiguration());
            modelBuilder.ApplyConfiguration(new RoomAmenityConfiguration());
            modelBuilder.ApplyConfiguration(new ImageConfiguration());
            modelBuilder.ApplyConfiguration(new PaymentConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}