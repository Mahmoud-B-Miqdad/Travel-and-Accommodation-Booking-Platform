using TravelEase.Domain.Aggregates.Discounts;

namespace TravelEase.Infrastructure.Common.Helpers
{
    public static class DiscountHelper
    {
        public static float GetActiveDiscount(List<Discount> roomTypeDiscounts)
        {
            var discount = roomTypeDiscounts
                .FirstOrDefault(d =>
                    d.FromDate.Date <= DateTime.Today.Date &&
                    d.ToDate.Date >= DateTime.Today.Date);

            if (discount == null)
                return 0.0f;

            var percentage = discount.DiscountPercentage;

            if (percentage > 1)
                percentage /= 100f;

            return Math.Clamp(percentage, 0f, 1f);
        }
    }
}