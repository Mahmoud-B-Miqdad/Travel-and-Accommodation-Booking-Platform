namespace TravelEase.Domain.Aggregates.Review
{
    public class Review
    {
        public Guid Id { get; set; }
        public string Comment { get; set; }
        public DateTime ReviewDate { get; set; }
        public float Rating { get; set; }
    }
}