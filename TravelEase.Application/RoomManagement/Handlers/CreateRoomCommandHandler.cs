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
        private readonly IHotelOwnershipValidator _hotelOwnershipValidator;
        private readonly IMapper _mapper;

        public CreateRoomCommandHandler
            (IUnitOfWork unitOfWork, IMapper mapper, IHotelOwnershipValidator hotelOwnershipValidator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _hotelOwnershipValidator = hotelOwnershipValidator;
        }

        public async Task<RoomResponse?> Handle(CreateRoomCommand request, CancellationToken cancellationToken)
        {
            if (!await _unitOfWork.Hotels.ExistsAsync(request.HotelId))
                throw new NotFoundException("Hotel doesn't exists.");

            if (!await _unitOfWork.RoomTypes.ExistsAsync(request.RoomTypeId))
                throw new NotFoundException("RoomCategory doesn't exists.");

            var belongs = await _hotelOwnershipValidator
                .IsRoomTypeBelongsToHotelAsync(request.RoomTypeId, request.HotelId);
            if (!belongs)
                throw new NotFoundException
                    ($"RoomType with ID {request.RoomTypeId} does not belong to hotel {request.HotelId}.");

            var roomToAdd = _mapper.Map<Room>(request);
            var addedRoom = await _unitOfWork.Rooms.AddAsync(roomToAdd);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return _mapper.Map<RoomResponse>(addedRoom);
        }
    }
}