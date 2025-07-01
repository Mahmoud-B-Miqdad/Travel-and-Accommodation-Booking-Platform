using AutoMapper;
using MediatR;
using TravelEase.Application.RoomAmenityManagement.Commands;
using TravelEase.Domain.Common.Interfaces;
using TravelEase.Domain.Exceptions;

namespace TravelEase.Application.RoomAmenityManagement.Handlers
{
    public class DeleteRoomAmenityCommandHandler : IRequestHandler<DeleteRoomAmenityCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DeleteRoomAmenityCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(DeleteRoomAmenityCommand request, CancellationToken cancellationToken)
        {
            var existingRoomAmenity = await _unitOfWork.RoomAmenities.GetByIdAsync(request.Id);
            if (existingRoomAmenity == null)
                throw new NotFoundException("Room Amenity Doesn't Exists To Delete");

            _unitOfWork.RoomAmenities.Remove(existingRoomAmenity);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}