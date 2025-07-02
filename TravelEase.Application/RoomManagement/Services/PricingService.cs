using TravelEase.Domain.Common.Helpers;
using TravelEase.Domain.Common.Interfaces;

namespace TravelEase.Application.RoomManagement.Services
{
    public class PricingService : IPricingService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PricingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<float> CalculateTotalPriceAsync(Guid roomId, DateTime checkInDate, DateTime checkOutDate)
        {
            var roomWithType = await _unitOfWork.Rooms.GetRoomWithTypeAndDiscountsAsync(roomId);
            if (roomWithType == null)
                throw new Exception("Room not found");

            var pricePerNight = roomWithType.RoomType.PricePerNight;
            var discounts = roomWithType.RoomType.Discounts;

            int totalDays = (checkOutDate.Date - checkInDate.Date).Days;
            if (totalDays <= 0)
                throw new InvalidOperationException("Check-out date must be after check-in date.");

            float totalPrice = 0;
            DateTime currentDate = checkInDate.Date;

            while (currentDate < checkOutDate.Date)
            {
                float discountPercentage = DiscountHelper.GetDiscountForDate(discounts, currentDate);
                float nightlyPrice = pricePerNight * (1 - discountPercentage);
                totalPrice += nightlyPrice;
                currentDate = currentDate.AddDays(1);
            }

            return totalPrice;
        }
    }
}