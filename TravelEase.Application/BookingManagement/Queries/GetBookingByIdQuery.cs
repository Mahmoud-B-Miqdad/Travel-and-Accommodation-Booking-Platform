using MediatR;
using TravelEase.Application.BookingManagement.DTOs.Responses;

namespace TravelEase.Application.BookingManagement.Queries
{
    public record GetBookingByIdQuery : IRequest<BookingResponse?>
    {
        public Guid Id { get; set; }
    }
}