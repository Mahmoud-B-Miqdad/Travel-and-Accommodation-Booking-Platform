using AutoMapper;
using MediatR;
using TravelEase.Application.HotelManagement.DTOs.Responses;
using TravelEase.Application.HotelManagement.Queries;
using TravelEase.Domain.Common.Interfaces;

namespace TravelEase.Application.HotelManagement.Handlers
{
    public class GetRecentlyVisitedHotelsForAuthenticatedGuestQueryHandler :
    IRequestHandler<GetRecentlyVisitedHotelsForAuthenticatedGuestQuery, List<HotelWithoutRoomsResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetRecentlyVisitedHotelsForAuthenticatedGuestQueryHandler
            (IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<HotelWithoutRoomsResponse>> Handle
            (GetRecentlyVisitedHotelsForAuthenticatedGuestQuery request,
            CancellationToken cancellationToken)
        {
            return _mapper.Map<List<HotelWithoutRoomsResponse>>
            (await _unitOfWork.Hotels
            .GetRecentlyVisitedHotelsForAuthenticatedGuestAsync
            (request.GuestEmail, request.Count));
        }
    }
}