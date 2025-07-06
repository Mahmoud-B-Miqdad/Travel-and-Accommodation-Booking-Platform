using AutoMapper;
using MediatR;
using TravelEase.Application.RoomManagement.DTOs.Responses;
using TravelEase.Application.RoomManagement.Queries;
using TravelEase.Domain.Common.Interfaces;

namespace TravelEase.Application.RoomManagement.Handlers
{
    public class GetHotelAvailableRoomsQueryHandler : 
        IRequestHandler<GetHotelAvailableRoomsQuery, List<RoomResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetHotelAvailableRoomsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<RoomResponse>> Handle(GetHotelAvailableRoomsQuery request, CancellationToken cancellationToken)
        {
            return _mapper.Map<List<RoomResponse>>(await _unitOfWork.Rooms
                .GetHotelAvailableRoomsAsync(
                request.HotelId,
                request.CheckInDate,
                request.CheckOutDate));
        }
    }
}