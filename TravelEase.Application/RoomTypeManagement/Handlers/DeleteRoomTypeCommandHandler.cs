using MediatR;
using TravelEase.Application.RoomTypeManagement.Commands;
using TravelEase.Domain.Aggregates.RoomTypes;
using TravelEase.Domain.Common.Interfaces;
using TravelEase.Domain.Exceptions;

namespace TravelEase.Application.RoomTypeManagement.Handlers
{
    public class DeleteRoomTypeCommandHandler : IRequestHandler<DeleteRoomTypeCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOwnershipValidator _hotelOwnershipValidator;

        public DeleteRoomTypeCommandHandler
            (IUnitOfWork unitOfWork, IOwnershipValidator hotelOwnershipValidator)
        {
            _unitOfWork = unitOfWork;
            _hotelOwnershipValidator = hotelOwnershipValidator;
        }

        public async Task Handle(DeleteRoomTypeCommand request, CancellationToken cancellationToken)
        {
            await EnsureHotelExists(request.HotelId);
            var roomType = await GetExistingRoomType(request.RoomTypeId);
            await EnsureRoomTypeBelongsToHotel(request.RoomTypeId, request.HotelId);
            await EnsureRoomTypeIsNotUsedInBookings(request.RoomTypeId);

            _unitOfWork.RoomTypes.Remove(roomType);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        private async Task EnsureHotelExists(Guid hotelId)
        {
            if (!await _unitOfWork.Hotels.ExistsAsync(hotelId))
                throw new NotFoundException("Hotel doesn't exist.");
        }

        private async Task<RoomType> GetExistingRoomType(Guid roomTypeId)
        {
            var roomType = await _unitOfWork.RoomTypes.GetByIdAsync(roomTypeId);
            if (roomType == null)
                throw new NotFoundException("RoomType doesn't exist to delete.");
            return roomType;
        }

        private async Task EnsureRoomTypeBelongsToHotel(Guid roomTypeId, Guid hotelId)
        {
            var belongs = await _hotelOwnershipValidator
                .IsRoomTypeBelongsToHotelAsync(roomTypeId, hotelId);
            if (!belongs)
                throw new NotFoundException
                    ($"RoomType with ID {roomTypeId} does not belong to hotel {hotelId}.");
        }

        private async Task EnsureRoomTypeIsNotUsedInBookings(Guid roomTypeId)
        {
            var hasBookings = await _unitOfWork.RoomTypes.HasActiveBookingsAsync(roomTypeId);
            if (hasBookings)
                throw new ConflictException
                    ("Cannot delete RoomType because it is used in existing bookings.");
        }
    }
}