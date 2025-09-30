using MediatR;
using TravelEase.Application.BookingManagement.DTOs.Responses;
using TravelEase.Domain.Common.Models.PaginationModels;

namespace TravelEase.Application.BookingManagement.Queries
{
    public record GetAllBookingsByHotelIdQuery : IRequest<PaginatedList<BookingResponse>>
    {
        public Guid HotelId { get; init; }
        public int PageNumber { get; init; }
        public int PageSize { get; init; }
    }
}