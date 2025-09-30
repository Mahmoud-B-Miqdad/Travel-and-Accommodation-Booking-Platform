using AutoMapper;
using MediatR;
using TravelEase.Application.RoomTypeManagement.DTOs.Responses;
using TravelEase.Application.RoomTypeManagement.Queries;
using TravelEase.Domain.Aggregates.RoomTypes;
using TravelEase.Domain.Common.Interfaces;
using TravelEase.Domain.Exceptions;

namespace TravelEase.Application.RoomTypeManagement.Handlers
{
    public class GetRoomTypeByIdAndHotelIdQueryHandler :
        IRequestHandler<GetRoomTypeByIdAndHotelIdQuery, RoomTypeResponse?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOwnershipValidator _hotelOwnershipValidator;
        private readonly IMapper _mapper;

        public GetRoomTypeByIdAndHotelIdQueryHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IOwnershipValidator hotelOwnershipValidator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _hotelOwnershipValidator = hotelOwnershipValidator;
        }

        public async Task<RoomTypeResponse?> Handle
            (GetRoomTypeByIdAndHotelIdQuery request, CancellationToken cancellationToken)
        {
            await EnsureHotelExistsAsync(request.HotelId);
            await EnsureRoomTypeBelongsToHotelAsync(request.RoomTypeId, request.HotelId);

            var roomType = await GetRoomTypeOrThrowAsync(request.RoomTypeId);

            return _mapper.Map<RoomTypeResponse>(roomType);
        }

        private async Task EnsureHotelExistsAsync(Guid hotelId)
        {
            if (!await _unitOfWork.Hotels.ExistsAsync(hotelId))
                throw new NotFoundException($"Hotel with ID {hotelId} doesn't exist.");
        }

        private async Task EnsureRoomTypeBelongsToHotelAsync(Guid roomTypeId, Guid hotelId)
        {
            if (!await _hotelOwnershipValidator.IsRoomTypeBelongsToHotelAsync(roomTypeId, hotelId))
                throw new NotFoundException
                    ($"RoomType with ID {roomTypeId} does not belong to hotel {hotelId}.");
        }

        private async Task<RoomType> GetRoomTypeOrThrowAsync(Guid roomTypeId)
        {
            var roomType = await _unitOfWork.RoomTypes.GetByIdAsync(roomTypeId);
            if (roomType == null)
                throw new NotFoundException($"RoomType with Id {roomTypeId} was not found.");
            return roomType;
        }
    }
}