using TravelEase.Domain.Aggregates.Bookings;

namespace TravelEase.Domain.Aggregates.Rooms
{
    public class Room
    {
        public Guid Id { get; set; }
        public List<Booking> Bookings { get; set; }
        public int AdultsCapacity { get; set; }
        public int ChildrenCapacity { get; set; }
        public string View { get; set; }
        public float Rating { get; set; }
    }
}