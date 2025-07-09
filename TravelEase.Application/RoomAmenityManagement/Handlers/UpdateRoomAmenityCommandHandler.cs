using AutoMapper;
using MediatR;
using TravelEase.Application.RoomAmenityManagement.Commands;
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
            await EnsureRoomAmenityExistsAsync(request.Id);
            await EnsureNameIsUniqueAsync(request.Name);

            var roomAmenityToUpdate = _mapper.Map<RoomAmenity>(request);
            _unitOfWork.RoomAmenities.Update(roomAmenityToUpdate);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        private async Task EnsureRoomAmenityExistsAsync(Guid roomAmenityId)
        {
            if (!await _unitOfWork.RoomAmenities.ExistsAsync(roomAmenityId))
                throw new NotFoundException($"RoomAmenity with ID {roomAmenityId} doesn't exist to update.");
        }

        private async Task EnsureNameIsUniqueAsync(string name)
        {
            if (await _unitOfWork.RoomAmenities.ExistsAsync(name))
                throw new ConflictException($"Another RoomAmenity with name '{name}' already exists.");
        }
    }
}