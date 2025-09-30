using AutoMapper;
using MediatR;
using TravelEase.Application.BookingManagement.DTOs.Responses;
using TravelEase.Application.DiscountManagement.DTOs.Responses;
using TravelEase.Application.DiscountManagement.Queries;
using TravelEase.Domain.Aggregates.Discounts;
using TravelEase.Domain.Common.Interfaces;
using TravelEase.Domain.Exceptions;

namespace TravelEase.Application.DiscountManagement.Handlers
{
    public class GetDiscountByIdAndRoomTypeIdQueryHandler : 
        IRequestHandler<GetDiscountByIdAndRoomTypeIdQuery, DiscountResponse?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOwnershipValidator _roomTypeOwnershipValidator;
        private readonly IMapper _mapper;

        public GetDiscountByIdAndRoomTypeIdQueryHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IOwnershipValidator roomTypeOwnershipValidator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _roomTypeOwnershipValidator = roomTypeOwnershipValidator;
        }

        public async Task<DiscountResponse?> Handle
            (GetDiscountByIdAndRoomTypeIdQuery request, CancellationToken cancellationToken)
        {
            await EnsureRoomTypeExistsAsync(request.RoomTypeId);
            await EnsureDiscountBelongsToRoomTypeAsync(request.DiscountId, request.RoomTypeId);

            var discount = await GetDiscountAsync(request.DiscountId);
            return _mapper.Map<DiscountResponse>(discount);
        }

        private async Task EnsureRoomTypeExistsAsync(Guid roomTypeId)
        {
            if (!await _unitOfWork.RoomTypes.ExistsAsync(roomTypeId))
                throw new NotFoundException($"Room Type with ID {roomTypeId} doesn't exist.");
        }

        private async Task EnsureDiscountBelongsToRoomTypeAsync(Guid discountId, Guid roomTypeId)
        {
            var belongs = await _roomTypeOwnershipValidator
                .IsDiscountBelongsToRoomTypeAsync(discountId, roomTypeId);
            if (!belongs)
                throw new NotFoundException($"Discount with ID {discountId} does not belong to roomType {roomTypeId}.");
        }

        private async Task<Discount> GetDiscountAsync(Guid discountId)
        {
            var discount = await _unitOfWork.Discounts.GetByIdAsync(discountId);
            if (discount == null)
                throw new NotFoundException($"Discount with ID {discountId} was not found.");

            return discount;
        }
    }
}