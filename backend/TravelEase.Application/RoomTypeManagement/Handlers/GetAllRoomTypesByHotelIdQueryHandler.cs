using AutoMapper;
using MediatR;
using TravelEase.Application.RoomTypeManagement.DTOs.Responses;
using TravelEase.Application.RoomTypeManagement.Queries;
using TravelEase.Domain.Common.Interfaces;
using TravelEase.Domain.Common.Models.PaginationModels;
using TravelEase.Domain.Exceptions;

namespace TravelEase.Application.RoomTypeManagement.Handlers
{
    public class GetAllRoomTypesByHotelIdQueryHandler :
        IRequestHandler<GetAllRoomTypesByHotelIdQuery, PaginatedList<RoomTypeResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllRoomTypesByHotelIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginatedList<RoomTypeResponse>> Handle
            (GetAllRoomTypesByHotelIdQuery request,
            CancellationToken cancellationToken)
        {
            await EnsureHotelExistsAsync(request.HotelId);

            var paginatedList = await
                _unitOfWork.RoomTypes
                    .GetAllByHotelIdAsync(
                        request.HotelId,
                        request.IncludeAmenities,
                        request.PageNumber,
                        request.PageSize);

            return new PaginatedList<RoomTypeResponse>(
                _mapper.Map<List<RoomTypeResponse>>(paginatedList.Items),
                paginatedList.PageData);
        }

        private async Task EnsureHotelExistsAsync(Guid hotelId)
        {
            if (!await _unitOfWork.Hotels.ExistsAsync(hotelId))
                throw new NotFoundException("Hotel doesn't exist.");
        }
    }
}