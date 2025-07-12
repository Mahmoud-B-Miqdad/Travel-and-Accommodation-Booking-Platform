using MediatR;
using TravelEase.Application.DiscountManagement.Commands;
using TravelEase.Domain.Aggregates.Discounts;
using TravelEase.Domain.Common.Interfaces;
using TravelEase.Domain.Exceptions;

namespace TravelEase.Application.DiscountManagement.Handlers
{
    public class DeleteDiscountCommandHandler : IRequestHandler<DeleteDiscountCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOwnershipValidator _roomTypeOwnershipValidator;

        public DeleteDiscountCommandHandler
            (IUnitOfWork unitOfWork, IOwnershipValidator roomTypeOwnershipValidator)
        {
            _unitOfWork = unitOfWork;
            _roomTypeOwnershipValidator = roomTypeOwnershipValidator;
        }

        public async Task Handle(DeleteDiscountCommand request, CancellationToken cancellationToken)
        {
            await EnsureRoomTypeExists(request.RoomTypeId);
            var discount = await GetExistingDiscount(request.DiscountId);
            await EnsureDiscountBelongsToRoomType(request.DiscountId, request.RoomTypeId);

            _unitOfWork.Discounts.Remove(discount);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        private async Task EnsureRoomTypeExists(Guid roomTypeId)
        {
            if (!await _unitOfWork.RoomTypes.ExistsAsync(roomTypeId))
                throw new NotFoundException("RoomType doesn't exist.");
        }

        private async Task<Discount> GetExistingDiscount(Guid discountId)
        {
            var discount = await _unitOfWork.Discounts.GetByIdAsync(discountId);
            if (discount == null)
                throw new NotFoundException("Discount doesn't exist to delete.");
            return discount;
        }

        private async Task EnsureDiscountBelongsToRoomType(Guid discountId, Guid roomTypeId)
        {
            var belongs = await _roomTypeOwnershipValidator
                .IsDiscountBelongsToRoomTypeAsync(discountId, roomTypeId);
            if (!belongs)
                throw new NotFoundException
                    ($"Discount with ID {discountId} does not belong to roomType {roomTypeId}.");
        }
    }
}