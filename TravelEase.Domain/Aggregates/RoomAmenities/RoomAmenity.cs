using TravelEase.Domain.Aggregates.RoomTypes;

namespace TravelEase.Domain.Aggregates.RoomAmenities
{
    public class RoomAmenity
    {
        public Guid Id { get; set; }
        public List<RoomType> RoomTypes { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}