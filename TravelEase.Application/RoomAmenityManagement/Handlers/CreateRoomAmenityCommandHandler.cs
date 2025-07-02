using AutoMapper;
using MediatR;
using TravelEase.Application.RoomAmenityManagement.Commands;
using TravelEase.Application.RoomAmenityManagement.DTOs.Responses;
using TravelEase.Domain.Aggregates.RoomAmenities;
using TravelEase.Domain.Common.Interfaces;
using TravelEase.Domain.Exceptions;

namespace TravelEase.Application.RoomAmenityManagement.Handlers
{
    public class CreateRoomAmenityCommandHandler : IRequestHandler<CreateRoomAmenityCommand, RoomAmenityResponse?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateRoomAmenityCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<RoomAmenityResponse?> Handle(CreateRoomAmenityCommand request, CancellationToken cancellationToken)
        {
            var existingRoomAmenity = await _unitOfWork.Hotels.ExistsAsync(request.Name);
            if (existingRoomAmenity)
                throw new ConflictException($"RoomAmenity with name '{request.Name}' already exists.");

            var roomAmenityToAdd = _mapper.Map<RoomAmenity>(request);
            var addedRoomAmenity = await _unitOfWork.RoomAmenities.AddAsync(roomAmenityToAdd);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return _mapper.Map<RoomAmenityResponse>(addedRoomAmenity);
        }
    }
}