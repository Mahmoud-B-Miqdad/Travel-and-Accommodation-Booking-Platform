using AutoMapper;
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
        private readonly IHotelOwnershipValidator _hotelOwnershipValidator;
        private readonly IMapper _mapper;

        public GetRoomByIdAndHotelIdQueryHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IHotelOwnershipValidator hotelOwnershipValidator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _hotelOwnershipValidator = hotelOwnershipValidator;
        }

        public async Task<RoomResponse?> Handle(GetRoomByIdAndHotelIdQuery request, CancellationToken cancellationToken)
        {
            await EnsureHotelExistsAsync(request.HotelId);
            await EnsureRoomBelongsToHotelAsync(request.RoomId, request.HotelId);

            var room = await GetRoomOrThrowAsync(request.RoomId);

            return _mapper.Map<RoomResponse>(room);
        }

        private async Task EnsureHotelExistsAsync(Guid hotelId)
        {
            if (!await _unitOfWork.Hotels.ExistsAsync(hotelId))
                throw new NotFoundException($"Hotel with ID {hotelId} doesn't exist.");
        }

        private async Task EnsureRoomBelongsToHotelAsync(Guid roomId, Guid hotelId)
        {
            if (!await _hotelOwnershipValidator.IsRoomBelongsToHotelAsync(roomId, hotelId))
                throw new NotFoundException($"Room with ID {roomId} does not belong to hotel {hotelId}.");
        }

        private async Task<Domain.Aggregates.Rooms.Room> GetRoomOrThrowAsync(Guid roomId)
        {
            var room = await _unitOfWork.Rooms.GetByIdAsync(roomId);
            if (room == null)
                throw new NotFoundException($"Room with Id {roomId} was not found.");
            return room;
        }
    }
}