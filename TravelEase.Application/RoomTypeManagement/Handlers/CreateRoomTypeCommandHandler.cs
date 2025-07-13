using AutoMapper;
using MediatR;
using TravelEase.Application.RoomTypeManagement.Commands;
using TravelEase.Application.RoomTypeManagement.DTOs.Responses;
using TravelEase.Domain.Aggregates.RoomAmenities;
using TravelEase.Domain.Aggregates.RoomTypes;
using TravelEase.Domain.Common.Interfaces;
using TravelEase.Domain.Enums;
using TravelEase.Domain.Exceptions;

namespace TravelEase.Application.RoomTypeManagement.Handlers
{
    public class CreateRoomTypeCommandHandler
            : IRequestHandler<CreateRoomTypeCommand, RoomTypeResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateRoomTypeCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<RoomTypeResponse?> Handle(CreateRoomTypeCommand request, CancellationToken cancellationToken)
        {
            await EnsureHotelExistsAsync(request.HotelId);
            await EnsureRoomTypeNotDuplicateAsync(request.HotelId, request.Category);

            var roomTypeToAdd = _mapper.Map<RoomType>(request);

            if (request.AmenityIds.Any())
            {
                var amenities = await GetValidAmenitiesAsync(request.AmenityIds);
                roomTypeToAdd.Amenities = amenities;
            }

            var addedRoomType = await _unitOfWork.RoomTypes.AddAsync(roomTypeToAdd);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return _mapper.Map<RoomTypeResponse>(addedRoomType);
        }

        private async Task EnsureHotelExistsAsync(Guid hotelId)
        {
            if (!await _unitOfWork.Hotels.ExistsAsync(hotelId))
                throw new NotFoundException("Hotel doesn't exist.");
        }

        private async Task<List<RoomAmenity>> GetValidAmenitiesAsync(List<Guid> amenityIds)
        {
            var amenities = await _unitOfWork.RoomAmenities.GetByIdsAsync(amenityIds);

            if (amenities.Count != amenityIds.Count)
                throw new NotFoundException("One or more amenities were not found.");

            return amenities;
        }

        private async Task EnsureRoomTypeNotDuplicateAsync(Guid hotelId, RoomCategory category)
        {
            var exists = await _unitOfWork.RoomTypes
                .ExistsByHotelAndCategoryAsync(hotelId, category);

            if (exists)
                throw new ConflictException
                    ($"RoomType of category '{category}' already exists for this hotel.");
        }
    }
}