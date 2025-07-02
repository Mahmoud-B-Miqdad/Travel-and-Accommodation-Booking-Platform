using TravelEase.Domain.Aggregates.Discounts;

namespace TravelEase.Infrastructure.Common.Helpers
{
    public static class DiscountHelper
    {
        public static float GetActiveDiscount(List<Discount> roomTypeDiscount)
        {
            return roomTypeDiscount
                .FirstOrDefault(discount =>
                    discount.FromDate.Date <= DateTime.Today.Date &&
                    discount.ToDate.Date >= DateTime.Today.Date)
                ?.DiscountPercentage ?? 0.0f;
        }
    }
}