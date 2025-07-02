using TravelEase.Domain.Aggregates.Discounts;

namespace TravelEase.Domain.Common.Helpers
{
    public static class DiscountHelper
    {
        public static float GetDiscountForDate(List<Discount> discounts, DateTime date)
        {
            var discount = discounts
                .FirstOrDefault(d =>
                    d.FromDate.Date <= date.Date &&
                    d.ToDate.Date >= date.Date);

            if (discount == null)
                return 0.0f;

            var percentage = discount.DiscountPercentage;

            if (percentage > 1)
                percentage /= 100f;

            return Math.Clamp(percentage, 0f, 1f);
        }
    }
}