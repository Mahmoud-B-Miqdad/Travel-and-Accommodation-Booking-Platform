﻿using TravelEase.Domain.Common.Interfaces;
using TravelEase.Domain.Common.Models.CommonModels;
using TravelEase.Domain.Common.Models.PaginationModels;

namespace TravelEase.Domain.Aggregates.Bookings
{
    public interface IBookingRepository : ICrudRepository<Booking>
    {
        Task<PaginatedList<Booking>> GetAllByHotelIdAsync(Guid hotelId, int pageNumber, int pageSize);
        Task<bool> IsBookingAccessibleToUserAsync(Guid bookingId, string guestEmail);
        Task<bool> ExistsConflictingBookingAsync(Guid roomId, DateTime checkInDate, 
            DateTime checkOutDate);
        Task<Invoice?> GetInvoiceByBookingIdAsync(Guid bookingId);
    }
}