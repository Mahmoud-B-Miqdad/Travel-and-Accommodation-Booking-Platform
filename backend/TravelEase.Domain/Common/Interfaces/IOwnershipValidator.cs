namespace TravelEase.Domain.Common.Interfaces
{
    public interface IOwnershipValidator
    {
        Task<bool> IsRoomBelongsToHotelAsync(Guid roomId, Guid hotelId);
        Task<bool> IsBookingBelongsToHotelAsync(Guid bookingId, Guid hotelId);
        Task<bool> IsReviewBelongsToHotelAsync(Guid reviewId, Guid hotelId);
        Task<bool> IsRoomTypeBelongsToHotelAsync(Guid roomTypeId, Guid hotelId);
        Task<bool> IsDiscountBelongsToRoomTypeAsync(Guid discountId, Guid roomTypeId);
    }
}