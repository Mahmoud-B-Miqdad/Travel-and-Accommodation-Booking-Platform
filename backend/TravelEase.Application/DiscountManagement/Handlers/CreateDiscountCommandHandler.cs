using AutoMapper;
using MediatR;
using TravelEase.Application.DiscountManagement.Commands;
using TravelEase.Application.DiscountManagement.DTOs.Responses;
using TravelEase.Domain.Aggregates.Discounts;
using TravelEase.Domain.Common.Interfaces;
using TravelEase.Domain.Exceptions;

namespace TravelEase.Application.DiscountManagement.Handlers
{
    public class CreateDiscountCommandHandler
         : IRequestHandler<CreateDiscountCommand, DiscountResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateDiscountCommandHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<DiscountResponse?> Handle
            (CreateDiscountCommand request, CancellationToken cancellationToken)
        {
            await EnsureRoomTypeExistsAsync(request.RoomTypeId);
            await EnsureNoConflictingDiscountAsync
                (request.RoomTypeId, request.FromDate, request.ToDate);

            var discountToAdd = _mapper.Map<Discount>(request);
            var addedDiscount = await _unitOfWork.Discounts.AddAsync(discountToAdd);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return _mapper.Map<DiscountResponse>(addedDiscount);
        }

        private async Task EnsureRoomTypeExistsAsync(Guid roomTypeId)
        {
            if (!await _unitOfWork.RoomTypes.ExistsAsync(roomTypeId))
                throw new NotFoundException("RoomType doesn't exist.");
        }

        private async Task EnsureNoConflictingDiscountAsync
            (Guid roomTypeId, DateTime fromDate, DateTime toDate)
        {
            var isConflict = await _unitOfWork.Discounts
                .ExistsConflictingDiscountAsync(roomTypeId, fromDate, toDate);
            if (isConflict)
            {
                var msg = $"There is already an overlapping discount for the same room type.";
                throw new ConflictException(msg);
            }
        }
    }
}