using TravelEase.Domain.Aggregates.Reviews;
using TravelEase.Domain.Aggregates.RoomTypes;

namespace TravelEase.Domain.Aggregates.Discounts
{
    public class Discount
    {
        public Guid Id { get; set; }
        public Guid RoomTypeId { get; set; }
        public RoomType RoomType { get; set; }
        public float DiscountPercentage { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
}