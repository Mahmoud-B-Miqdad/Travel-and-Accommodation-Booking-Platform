namespace TravelEase.Domain.Common.Interfaces
{
    public interface IPricingService
    {
        Task<float> CalculateTotalPriceAsync(Guid roomId, DateTime checkInDate, DateTime checkOutDate);
    }
}