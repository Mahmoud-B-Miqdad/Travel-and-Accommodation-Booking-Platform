using AutoMapper;
using MediatR;
using TravelEase.Application.HotelManagement.DTOs.Responses;
using TravelEase.Application.HotelManagement.Queries;
using TravelEase.Domain.Common.Interfaces;
using TravelEase.Domain.Common.Models.PaginationModels;

namespace TravelEase.Application.HotelManagement.Handlers
{
    public class GetAllHotelsQueryHandler : IRequestHandler<GetAllHotelsQuery, PaginatedList<HotelWithoutRoomsResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllHotelsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginatedList<HotelWithoutRoomsResponse>> Handle
            (GetAllHotelsQuery request, CancellationToken cancellationToken)
        {
            var paginatedList = await
                _unitOfWork.Hotels
                    .GetAllAsync(
                        request.SearchQuery,
                        request.PageNumber,
                        request.PageSize);

            return new PaginatedList<HotelWithoutRoomsResponse>(
                _mapper.Map<List<HotelWithoutRoomsResponse>>(paginatedList.Items),
                paginatedList.PageData);
        }
    }
}