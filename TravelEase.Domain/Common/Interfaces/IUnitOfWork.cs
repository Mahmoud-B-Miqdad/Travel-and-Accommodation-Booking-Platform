using TravelEase.Domain.Aggregates.Bookings;
using TravelEase.Domain.Aggregates.Cities;
using TravelEase.Domain.Aggregates.Discounts;
using TravelEase.Domain.Aggregates.Hotels;
using TravelEase.Domain.Aggregates.Reviews;
using TravelEase.Domain.Aggregates.RoomAmenities;
using TravelEase.Domain.Aggregates.Rooms;
using TravelEase.Domain.Aggregates.RoomTypes;
using TravelEase.Domain.Aggregates.Users;

namespace TravelEase.Domain.Common.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IBookingRepository Bookings { get; }
        IRoomRepository Rooms { get; }
        ICityRepository Cities { get; }
        IHotelRepository Hotels { get; }
        IReviewRepository Reviews { get; }
        IUserRepository Users { get; }
        IRoomAmenityRepository RoomAmenities { get; }
        IDiscountRepository Discounts { get; }
        IRoomTypeRepository RoomTypes { get; }

        Task<int> SaveChangesAsync();
    }
}