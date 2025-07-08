using MediatR;
using TravelEase.Application.BookingManagement.DTOs.Responses;

namespace TravelEase.Application.BookingManagement.Queries
{
    public record GetBookingByIdAndHotelIdQuery : IRequest<BookingResponse?>
    {
        public Guid HotelId { get; init; }
        public Guid BookingId { get; init; }
    }
}