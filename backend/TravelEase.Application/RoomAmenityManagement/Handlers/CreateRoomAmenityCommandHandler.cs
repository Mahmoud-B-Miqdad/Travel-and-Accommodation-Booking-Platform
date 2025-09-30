using AutoMapper;
using MediatR;
using TravelEase.Application.RoomAmenityManagement.Commands;
using TravelEase.Application.RoomAmenityManagement.DTOs.Responses;
using TravelEase.Domain.Aggregates.RoomAmenities;
using TravelEase.Domain.Common.Interfaces;
using TravelEase.Domain.Exceptions;

namespace TravelEase.Application.RoomAmenityManagement.Handlers
{
    public class CreateRoomAmenityCommandHandler 
        : IRequestHandler<CreateRoomAmenityCommand, RoomAmenityResponse?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateRoomAmenityCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<RoomAmenityResponse?> Handle
            (CreateRoomAmenityCommand request, CancellationToken cancellationToken)
        {
            await EnsureAmenityDoesNotExistAsync(request.Name);

            var amenity = _mapper.Map<RoomAmenity>(request);
            var addedAmenity = await _unitOfWork.RoomAmenities.AddAsync(amenity);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return _mapper.Map<RoomAmenityResponse>(addedAmenity);
        }

        private async Task EnsureAmenityDoesNotExistAsync(string name)
        {
            if (await _unitOfWork.RoomAmenities.ExistsAsync(name))
                throw new ConflictException($"RoomAmenity with name '{name}' already exists.");
        }
    }
}