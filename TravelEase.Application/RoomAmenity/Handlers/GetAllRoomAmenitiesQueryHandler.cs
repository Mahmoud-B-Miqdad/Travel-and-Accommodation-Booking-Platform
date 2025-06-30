using AutoMapper;
using MediatR;
using TravelEase.Application.RoomAmenity.DTOs.Responses;
using TravelEase.Application.RoomAmenity.Query;
using TravelEase.Domain.Common.Interfaces;
using TravelEase.Domain.Common.Models.PaginationModels;

namespace TravelEase.Application.RoomAmenity.Handlers
{
    public class GetAllRoomAmenitiesQueryHandler : IRequestHandler<GetAllRoomAmenitiesQuery, PaginatedList<RoomAmenityResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllRoomAmenitiesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginatedList<RoomAmenityResponse>> Handle(GetAllRoomAmenitiesQuery request, CancellationToken cancellationToken)
        {
            var paginatedList = await
                _unitOfWork.RoomAmenities
                    .GetAllAsync(
                        request.SearchQuery,
                        request.PageNumber,
                        request.PageSize);

            return new PaginatedList<RoomAmenityResponse>(
                _mapper.Map<List<RoomAmenityResponse>>(paginatedList.Items),
                paginatedList.PageData);
        }
    }
}