using MediatR;
using TravelEase.Application.BookingManagement.DTOs.Responses;
using TravelEase.Domain.Common.Models.PaginationModels;

namespace TravelEase.Application.BookingManagement.Queries
{
    public class GetAllBookingsByHotelIdQuery : IRequest<PaginatedList<BookingResponse>>
    {
        public Guid HotelId { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}