using MediatR;
using TravelEase.Application.RoomAmenityManagement.Commands;
using TravelEase.Domain.Aggregates.RoomAmenities;
using TravelEase.Domain.Common.Interfaces;
using TravelEase.Domain.Exceptions;

namespace TravelEase.Application.RoomAmenityManagement.Handlers
{
    public class DeleteRoomAmenityCommandHandler : IRequestHandler<DeleteRoomAmenityCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteRoomAmenityCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(DeleteRoomAmenityCommand request, CancellationToken cancellationToken)
        {
            var amenity = await GetRoomAmenityOrThrowAsync(request.Id);
            _unitOfWork.RoomAmenities.Remove(amenity);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        private async Task<RoomAmenity> GetRoomAmenityOrThrowAsync(Guid id)
        {
            var amenity = await _unitOfWork.RoomAmenities.GetByIdAsync(id);
            if (amenity == null)
                throw new NotFoundException("Room Amenity doesn't exist to delete.");
            return amenity;
        }
    }
}