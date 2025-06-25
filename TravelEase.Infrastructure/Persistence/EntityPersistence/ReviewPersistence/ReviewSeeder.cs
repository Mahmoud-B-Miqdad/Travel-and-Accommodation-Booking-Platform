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
                    Id = Guid.Parse("0a1f221e-1a1e-44c2-9cfa-aaaabbbb0001"),
                    BookingId = Guid.Parse("de1c4d04-0c38-4d08-b0b7-0a1a16be1a11"),
                    Comment = "Amazing stay! Very clean and comfortable.",
                    ReviewDate = new DateTime(2025, 7, 16),
                    Rating = 4.8f
                },
                new Review
                {
                    Id = Guid.Parse("0a1f221e-1a1e-44c2-9cfa-aaaabbbb0002"),
                    BookingId = Guid.Parse("f3d0d7c9-c18b-45f6-86e3-f6b95e3f8cb7"),
                    Comment = "The service was good but the room was a bit small.",
                    ReviewDate = new DateTime(2025, 9, 11),
                    Rating = 3.7f
                },
                new Review
                {
                    Id = Guid.Parse("0a1f221e-1a1e-44c2-9cfa-aaaabbbb0003"),
                    BookingId = Guid.Parse("d248dd73-98a2-45e2-8e4e-6eb8b78cc006"),
                    Comment = "Excellent value for money. Will come again.",
                    ReviewDate = new DateTime(2025, 11, 16),
                    Rating = 5.0f
                }
            };
        }
    }
}