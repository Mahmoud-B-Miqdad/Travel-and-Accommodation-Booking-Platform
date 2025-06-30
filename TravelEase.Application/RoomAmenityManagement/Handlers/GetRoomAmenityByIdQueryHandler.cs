using AutoMapper;
using MediatR;
using TravelEase.Application.RoomAmenityManagement.DTOs.Responses;
using TravelEase.Application.RoomAmenityManagement.Query;
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
            var roomAmenity = await _unitOfWork.RoomAmenities.GetByIdAsync(request.Id);
            if (roomAmenity == null)
                throw new NotFoundException($"Amenity with Id {request.Id} was not found.");

            return _mapper.Map<RoomAmenityResponse>(roomAmenity);
        }
    }
}