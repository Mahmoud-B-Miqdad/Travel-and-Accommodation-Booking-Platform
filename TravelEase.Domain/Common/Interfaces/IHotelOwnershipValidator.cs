namespace TravelEase.Domain.Common.Interfaces
{
    public interface IHotelOwnershipValidator
    {
        Task<bool> IsRoomBelongsToHotelAsync(Guid roomId, Guid hotelId);
        Task<bool> IsBookingBelongsToHotelAsync(Guid bookingId, Guid hotelId);
        Task<bool> IsReviewBelongsToHotelAsync(Guid reviewId, Guid hotelId);
        Task<bool> IsRoomTypeBelongsToHotelAsync(Guid roomTypeId, Guid hotelId);
    }
}