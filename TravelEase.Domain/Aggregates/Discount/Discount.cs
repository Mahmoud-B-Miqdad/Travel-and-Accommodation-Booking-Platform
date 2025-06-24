namespace TravelEase.Domain.Aggregates.Discount
{
    public class Discount
    {
        public Guid Id { get; set; }
        public float DiscountPercentage { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
}