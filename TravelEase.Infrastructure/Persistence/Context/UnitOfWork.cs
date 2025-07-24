using TravelEase.Domain.Aggregates.Bookings;
using TravelEase.Domain.Aggregates.Cities;
using TravelEase.Domain.Aggregates.Discounts;
using TravelEase.Domain.Aggregates.Hotels;
using TravelEase.Domain.Aggregates.Payments;
using TravelEase.Domain.Aggregates.Reviews;
using TravelEase.Domain.Aggregates.RoomAmenities;
using TravelEase.Domain.Aggregates.Rooms;
using TravelEase.Domain.Aggregates.RoomTypes;
using TravelEase.Domain.Aggregates.Users;
using TravelEase.Domain.Common.Interfaces;

namespace TravelEase.Infrastructure.Persistence.Context
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TravelEaseDbContext _context;

        public IBookingRepository Bookings { get; private set; }
        public IRoomRepository Rooms { get; private set; }
        public ICityRepository Cities { get; private set; }
        public IHotelRepository Hotels { get; private set; }
        public IReviewRepository Reviews { get; private set; }
        public IUserRepository Users { get; private set; }
        public IRoomAmenityRepository RoomAmenities { get; private set; }
        public IDiscountRepository Discounts { get; private set; }
        public IRoomTypeRepository RoomTypes { get; private set; }
        public IPaymentRepository Payments { get; private set; }

        public UnitOfWork(TravelEaseDbContext context,
                          IBookingRepository bookings,
                          IRoomRepository rooms,
                          ICityRepository cities,
                          IHotelRepository hotels,
                          IReviewRepository reviews,
                          IUserRepository users,
                          IRoomAmenityRepository roomAmenities,
                          IDiscountRepository discounts,
                          IRoomTypeRepository roomTypes,
                          IPaymentRepository payments)
        {
            _context = context;
            Bookings = bookings;
            Rooms = rooms;
            Cities = cities;
            Hotels = hotels;
            Reviews = reviews;
            Users = users;
            RoomAmenities = roomAmenities;
            Discounts = discounts;
            RoomTypes = roomTypes;
            Payments = payments;
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}