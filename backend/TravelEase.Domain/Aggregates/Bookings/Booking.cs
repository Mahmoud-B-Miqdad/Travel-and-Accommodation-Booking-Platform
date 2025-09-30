using TravelEase.Domain.Aggregates.Payments;
using TravelEase.Domain.Aggregates.Reviews;
using TravelEase.Domain.Aggregates.Rooms;
using TravelEase.Domain.Aggregates.Users;

namespace TravelEase.Domain.Aggregates.Bookings
{
    public class Booking
    {
        public Guid Id { get; set; }
        public Guid RoomId { get; set; }
        public Room Room { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public Review? Review { get; set; }
        public Payment? Payment { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public DateTime BookingDate { get; set; }
        public double Price { get; set; }
    }
}