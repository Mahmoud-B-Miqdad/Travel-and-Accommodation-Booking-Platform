﻿using AutoMapper;
using MediatR;
using TravelEase.Application.RoomManagement.DTOs.Responses;
using TravelEase.Application.RoomManagement.Queries;
using TravelEase.Domain.Common.Interfaces;
using TravelEase.Domain.Exceptions;

namespace TravelEase.Application.RoomManagement.Handlers
{
    public class GetRoomByIdAndHotelIdQueryHandler : IRequestHandler<GetRoomByIdAndHotelIdQuery, RoomResponse?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetRoomByIdAndHotelIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<RoomResponse?> Handle(GetRoomByIdAndHotelIdQuery request,
            CancellationToken cancellationToken)
        {
            var hotelExists = await _unitOfWork.Hotels.ExistsAsync(request.HotelId);
            if (!hotelExists)
                throw new NotFoundException($"Hotel with ID {request.HotelId} doesn't exist.");

            var belongsToHotel = await _unitOfWork.Rooms.IsRoomBelongsToHotelAsync(request.RoomId, request.HotelId);
            if (!belongsToHotel)
                throw new NotFoundException("The room doesn't belong to the hotel.");

            var room = await _unitOfWork.Rooms.GetByIdAsync(request.RoomId);
            if (room == null)
                throw new NotFoundException($"Room with Id {request.RoomId} was not found.");

            return _mapper.Map<RoomResponse>(room);
        }
    }
}