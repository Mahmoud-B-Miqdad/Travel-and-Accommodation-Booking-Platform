using AutoMapper;
using MediatR;
using TravelEase.Application.HotelManagement.DTOs.Responses;
using TravelEase.Application.HotelManagement.Queries;
using TravelEase.Domain.Aggregates.Hotels;
using TravelEase.Domain.Aggregates.Users;
using TravelEase.Domain.Common.Interfaces;
using TravelEase.Domain.Exceptions;

namespace TravelEase.Application.HotelManagement.Handlers
{
    public class GetRecentlyVisitedHotelsForGuestQueryHandler :
    IRequestHandler<GetRecentlyVisitedHotelsForGuestQuery, List<HotelWithoutRoomsResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetRecentlyVisitedHotelsForGuestQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;

        }

        public async Task<List<HotelWithoutRoomsResponse>> Handle
            (GetRecentlyVisitedHotelsForGuestQuery request,CancellationToken cancellationToken)
        {
            if (!await _unitOfWork.Users.ExistsAsync(request.GuestId))
            {
                throw new NotFoundException($"User With ID {request.GuestId} Doesn't Exists.");
            }

            return _mapper.Map<List<HotelWithoutRoomsResponse>>
            (await _unitOfWork.Hotels
            .GetRecentlyVisitedHotelsForGuestAsync
            (request.GuestId, request.Count));
        }
    }
}