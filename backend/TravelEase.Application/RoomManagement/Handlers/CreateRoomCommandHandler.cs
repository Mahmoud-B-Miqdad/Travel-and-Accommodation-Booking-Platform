using AutoMapper;
using MediatR;
using TravelEase.Application.RoomManagement.Commands;
using TravelEase.Application.RoomManagement.DTOs.Responses;
using TravelEase.Domain.Aggregates.Rooms;
using TravelEase.Domain.Common.Interfaces;
using TravelEase.Domain.Exceptions;

namespace TravelEase.Application.RoomManagement.Handlers
{
    public class CreateRoomCommandHandler : IRequestHandler<CreateRoomCommand, RoomResponse?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOwnershipValidator _hotelOwnershipValidator;
        private readonly IMapper _mapper;

        public CreateRoomCommandHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IOwnershipValidator hotelOwnershipValidator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _hotelOwnershipValidator = hotelOwnershipValidator;
        }

        public async Task<RoomResponse?> Handle(CreateRoomCommand request, CancellationToken cancellationToken)
        {
            await EnsureHotelExistsAsync(request.HotelId);
            await EnsureRoomTypeExistsAsync(request.RoomTypeId);
            await EnsureRoomTypeBelongsToHotelAsync(request.RoomTypeId, request.HotelId);

            var roomToAdd = _mapper.Map<Room>(request);
            var addedRoom = await _unitOfWork.Rooms.AddAsync(roomToAdd);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return _mapper.Map<RoomResponse>(addedRoom);
        }

        private async Task EnsureHotelExistsAsync(Guid hotelId)
        {
            if (!await _unitOfWork.Hotels.ExistsAsync(hotelId))
                throw new NotFoundException("Hotel doesn't exist.");
        }

        private async Task EnsureRoomTypeExistsAsync(Guid roomTypeId)
        {
            if (!await _unitOfWork.RoomTypes.ExistsAsync(roomTypeId))
                throw new NotFoundException("Room category doesn't exist.");
        }

        private async Task EnsureRoomTypeBelongsToHotelAsync(Guid roomTypeId, Guid hotelId)
        {
            if (!await _hotelOwnershipValidator.IsRoomTypeBelongsToHotelAsync(roomTypeId, hotelId))
                throw new NotFoundException($"RoomType with ID {roomTypeId} does not belong to hotel {hotelId}.");
        }
    }
}