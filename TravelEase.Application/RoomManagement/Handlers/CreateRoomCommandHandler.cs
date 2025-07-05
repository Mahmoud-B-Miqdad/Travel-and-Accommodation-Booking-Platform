using AutoMapper;
using TravelEase.Application.RoomManagement.Commands;
using TravelEase.Application.RoomManagement.DTOs.Responses;
using TravelEase.Domain.Aggregates.Rooms;
using TravelEase.Domain.Common.Interfaces;
using TravelEase.Domain.Exceptions;

namespace TravelEase.Application.RoomManagement.Handlers
{
    public class CreateRoomCommandHandler
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateRoomCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<RoomResponse?> Handle(CreateRoomCommand request, CancellationToken cancellationToken)
        {
            if (!await _unitOfWork.Hotels.ExistsAsync(request.HotelId))
                throw new NotFoundException("Hotel doesn't exists.");

            if (!await _unitOfWork.RoomTypes.ExistsAsync(request.RoomTypeId))
                throw new NotFoundException("RoomCategory doesn't exists.");

            if (!await _unitOfWork.RoomTypes
                .CheckRoomTypeExistenceForHotelAsync(request.HotelId, request.RoomTypeId))
                throw new NotFoundException("The specified room category does not exist for the given hotel.");

            var roomToAdd = _mapper.Map<Room>(request);
            var addedRoom = await _unitOfWork.Rooms.AddAsync(roomToAdd);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return _mapper.Map<RoomResponse>(addedRoom);
        }
    }
}