using AutoMapper;
using MediatR;
using TravelEase.Application.RoomManagement.DTOs.Responses;
using TravelEase.Application.RoomManagement.Queries;
using TravelEase.Domain.Common.Interfaces;
using TravelEase.Domain.Common.Models.PaginationModels;
using TravelEase.Domain.Exceptions;

namespace TravelEase.Application.RoomManagement.Handlers
{
    public class GetAllRoomsByHotelIdQueryHandler : 
        IRequestHandler<GetAllRoomsByHotelIdQuery, PaginatedList<RoomResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetAllRoomsByHotelIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginatedList<RoomResponse>> Handle
            (GetAllRoomsByHotelIdQuery request,
            CancellationToken cancellationToken)
        {
            await EnsureHotelExistsAsync(request.HotelId);

            var paginatedList = await
                _unitOfWork.Rooms
                    .GetAllByHotelIdAsync(
                        request.HotelId,
                        request.SearchQuery,
                        request.PageNumber,
                        request.PageSize);

            return new PaginatedList<RoomResponse>(
                _mapper.Map<List<RoomResponse>>(paginatedList.Items),
                paginatedList.PageData);
        }

        private async Task EnsureHotelExistsAsync(Guid hotelId)
        {
            if (!await _unitOfWork.Hotels.ExistsAsync(hotelId))
                throw new NotFoundException("Hotel doesn't exist.");
        }
    }
}