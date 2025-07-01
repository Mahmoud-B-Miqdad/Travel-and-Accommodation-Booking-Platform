using AutoMapper;
using MediatR;
using TravelEase.Application.RoomAmenityManagement.Commands;
using TravelEase.Domain.Aggregates.Hotels;
using TravelEase.Domain.Aggregates.RoomAmenities;
using TravelEase.Domain.Common.Interfaces;
using TravelEase.Domain.Exceptions;

namespace TravelEase.Application.RoomAmenityManagement.Handlers
{

    public class UpdateRoomAmenityCommandHandler : IRequestHandler<UpdateRoomAmenityCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateRoomAmenityCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task Handle(UpdateRoomAmenityCommand request, CancellationToken cancellationToken)
        {
            var existingRoomAmenity = await _unitOfWork.RoomAmenities.IsExistsAsync(request.Id);
            if (!existingRoomAmenity)
                throw new NotFoundException($"Room Amenity With {request.Id} Doesn't Exists To Update");

            var conflictingRoomAmenity = await _unitOfWork.RoomAmenities.IsExistsAsync(request.Name);
            if (conflictingRoomAmenity)
                throw new ConflictException($"Another room amenity with name '{request.Name}' already exists.");

            var roomAmenityToUpdate = _mapper.Map<RoomAmenity>(request);
            _unitOfWork.RoomAmenities.Update(roomAmenityToUpdate);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}