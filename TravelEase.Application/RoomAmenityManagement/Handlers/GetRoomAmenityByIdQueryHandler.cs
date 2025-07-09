using AutoMapper;
using MediatR;
using TravelEase.Application.RoomAmenityManagement.DTOs.Responses;
using TravelEase.Application.RoomAmenityManagement.Query;
using TravelEase.Domain.Aggregates.RoomAmenities;
using TravelEase.Domain.Common.Interfaces;
using TravelEase.Domain.Exceptions;

namespace TravelEase.Application.RoomAmenityManagement.Handlers
{
    public class GetRoomAmenityByIdQueryHandler : IRequestHandler<GetRoomAmenityByIdQuery, RoomAmenityResponse?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetRoomAmenityByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<RoomAmenityResponse?> Handle(GetRoomAmenityByIdQuery request, CancellationToken cancellationToken)
        {
            var amenity = await GetRoomAmenityOrThrowAsync(request.Id);
            return _mapper.Map<RoomAmenityResponse>(amenity);
        }

        private async Task<RoomAmenity> GetRoomAmenityOrThrowAsync(Guid id)
        {
            var amenity = await _unitOfWork.RoomAmenities.GetByIdAsync(id);
            if (amenity == null)
                throw new NotFoundException($"Amenity with Id {id} was not found.");
            return amenity;
        }
    }
}