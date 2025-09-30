using AutoMapper;
using TravelEase.Application.BookingManagement.Commands;
using TravelEase.Application.BookingManagement.DTOs.Requests;
using TravelEase.Application.BookingManagement.DTOs.Responses;
using TravelEase.Application.BookingManagement.Queries;
using TravelEase.Domain.Aggregates.Bookings;
using TravelEase.Domain.Common.Models.CommonModels;

namespace TravelEase.Application.BookingManagement.Mapping
{
    public class BookingProfile : Profile
    {
        public BookingProfile()
        {
            CreateMap<BookingQueryRequest, GetAllBookingsByHotelIdQuery>();
            CreateMap<ReserveRoomRequest, ReserveRoomCommand>();
            CreateMap<ReserveRoomCommand, Booking>();
            CreateMap<Booking, BookingResponse>();
            CreateMap<Invoice, InvoiceResponse>();
            CreateMap<InvoiceResponse, Invoice>();
        }
    }
}