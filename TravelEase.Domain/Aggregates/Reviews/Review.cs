namespace TravelEase.Domain.Aggregates.Reviews
{
    public class Review
    {
        public Guid Id { get; set; }
        public string Comment { get; set; }
        public DateTime ReviewDate { get; set; }
        public float Rating { get; set; }
    }
}