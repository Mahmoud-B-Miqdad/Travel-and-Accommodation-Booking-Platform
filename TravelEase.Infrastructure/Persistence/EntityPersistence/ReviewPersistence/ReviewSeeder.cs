using TravelEase.Domain.Aggregates.Reviews;

namespace TravelEase.Infrastructure.Persistence.EntityPersistence.ReviewPersistence
{
    public static class ReviewSeeder
    {
        public static List<Review> GetSeedData()
        {
            return new List<Review>
            {
                new Review
                {
                    Id = new Guid("0a1f221e-1a1e-44c2-9cfa-aaaabbbb0001"),
                    BookingId = new Guid("45d3d4b7-65ef-4a3e-a19a-14a3f28a0001"), 
                    Comment = "Amazing stay! Very clean and comfortable.",
                    ReviewDate = new DateTime(2025, 7, 16),
                    Rating = 4.8f
                },
                new Review
                {
                    Id = new Guid("0a1f221e-1a1e-44c2-9cfa-aaaabbbb0002"),
                    BookingId = new Guid("6ed7464a-cb0e-4bf3-a03a-f7bb18e90003"), 
                    Comment = "The service was good but the room was a bit small.",
                    ReviewDate = new DateTime(2025, 9, 11),
                    Rating = 3.7f
                },
                new Review
                {
                    Id = new Guid("0a1f221e-1a1e-44c2-9cfa-aaaabbbb0003"),
                    BookingId = new Guid("eac18319-63a2-4cae-bc3e-cad2a45f0005"), 
                    Comment = "Excellent value for money. Will come again.",
                    ReviewDate = new DateTime(2025, 11, 16),
                    Rating = 5.0f
                }
            };
        }
    }
}