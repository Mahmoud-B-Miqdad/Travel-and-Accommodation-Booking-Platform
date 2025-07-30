using AutoMapper;
using MediatR;
using TravelEase.Application.RoomManagement.Commands;
using TravelEase.Domain.Aggregates.Rooms;
using TravelEase.Domain.Common.Interfaces;
using TravelEase.Domain.Exceptions;

namespace TravelEase.Application.RoomManagement.Handlers
{
    public class UpdateRoomCommandHandler : IRequestHandler<UpdateRoomCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOwnershipValidator _hotelOwnershipValidator;
        private readonly IMapper _mapper;

        public UpdateRoomCommandHandler(IUnitOfWork unitOfWork, IOwnershipValidator hotelOwnershipValidator,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _hotelOwnershipValidator = hotelOwnershipValidator;
        }

        public async Task Handle(UpdateRoomCommand request, CancellationToken cancellationToken)
        {
            await EnsureHotelExistsAsync(request.HotelId);
            await EnsureRoomExistsAsync(request.RoomId);
            await EnsureRoomBelongsToHotelAsync(request.RoomId, request.HotelId);

            var room = _mapper.Map<Room>(request);

            _unitOfWork.Rooms.Update(room);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        private async Task EnsureHotelExistsAsync(Guid hotelId)
        {
            if (!await _unitOfWork.Hotels.ExistsAsync(hotelId))
                throw new NotFoundException("Hotel doesn't exist.");
        }

        private async Task EnsureRoomExistsAsync(Guid roomId)
        {
            if (!await _unitOfWork.Rooms.ExistsAsync(roomId))
                throw new NotFoundException($"Room with ID {roomId} does not exist.");
        }

        private async Task EnsureRoomBelongsToHotelAsync(Guid roomId, Guid hotelId)
        {
            if (!await _hotelOwnershipValidator.IsRoomBelongsToHotelAsync(roomId, hotelId))
                throw new NotFoundException("Room does not belong to the specified hotel.");
        }
    }
}