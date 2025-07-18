﻿using AutoMapper;
using MediatR;
using TravelEase.Application.BookingManagement.DTOs.Responses;
using TravelEase.Application.BookingManagement.Queries;
using TravelEase.Domain.Common.Interfaces;
using TravelEase.Domain.Common.Models.PaginationModels;
using TravelEase.Domain.Exceptions;

namespace TravelEase.Application.BookingManagement.Handlers
{
    public class GetAllBookingsByHotelIdQueryHandler :
        IRequestHandler<GetAllBookingsByHotelIdQuery, PaginatedList<BookingResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllBookingsByHotelIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginatedList<BookingResponse>> Handle
            (GetAllBookingsByHotelIdQuery request,
            CancellationToken cancellationToken)
        {
            await EnsureHotelExistsAsync(request.HotelId);

            var paginatedList = await
                _unitOfWork.Bookings
                    .GetAllByHotelIdAsync(
                        request.HotelId,
                        request.PageNumber,
                        request.PageSize);

            return new PaginatedList<BookingResponse>(
                _mapper.Map<List<BookingResponse>>(paginatedList.Items),
                paginatedList.PageData);
        }

        private async Task EnsureHotelExistsAsync(Guid hotelId)
        {
            if (!await _unitOfWork.Hotels.ExistsAsync(hotelId))
                throw new NotFoundException("Hotel doesn't exist.");
        }
    }
}