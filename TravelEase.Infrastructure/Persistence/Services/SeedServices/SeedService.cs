using Microsoft.EntityFrameworkCore;
using TravelEase.Infrastructure.Persistence.Context;
using TravelEase.Infrastructure.Persistence.EntityPersistence.CityPersistence;
using TravelEase.Infrastructure.Persistence.EntityPersistence.DiscountPersistence;
using TravelEase.Infrastructure.Persistence.EntityPersistence.HotelPersistence;
using TravelEase.Infrastructure.Persistence.EntityPersistence.RoomAmenityPersistence;
using TravelEase.Infrastructure.Persistence.EntityPersistence.RoomPersistence;
using TravelEase.Infrastructure.Persistence.EntityPersistence.RoomTypePersistence;

namespace TravelEase.Infrastructure.Persistence.Services.SeedServices
{
    public class SeedService
    {
        private readonly TravelEaseDbContext _context;

        public SeedService(TravelEaseDbContext context)
        {
            _context = context;
        }

        public async Task SeedIfNeededAsync()
        {
            if (!await _context.Cities.AnyAsync())
            {
                _context.Cities.AddRange(CitySeeder.GetSeedData());
            }

            if (!await _context.Rooms.AnyAsync())
            {
                _context.Rooms.AddRange(RoomSeeder.GetSeedData());
            }

            if (!await _context.Hotels.AnyAsync())
            {
                _context.Hotels.AddRange(HotelSeeder.GetSeedData());
            }

            if (!await _context.Discounts.AnyAsync())
            {
                _context.Discounts.AddRange(DiscountSeeder.GetSeedData());
            }

            if (!await _context.RoomAmenities.AnyAsync())
            {
                _context.RoomAmenities.AddRange(RoomAmenitySeeder.GetSeedData());
            }

            if (!await _context.RoomTypes.AnyAsync())
            {
                _context.RoomTypes.AddRange(RoomTypeSeeder.GetSeedData());
            }

            await _context.SaveChangesAsync();
        }
    }
}