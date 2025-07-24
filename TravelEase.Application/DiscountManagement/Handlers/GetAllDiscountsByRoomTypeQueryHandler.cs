using AutoMapper;
using MediatR;
using TravelEase.Application.DiscountManagement.DTOs.Responses;
using TravelEase.Application.DiscountManagement.Queries;
using TravelEase.Domain.Common.Interfaces;
using TravelEase.Domain.Common.Models.PaginationModels;
using TravelEase.Domain.Exceptions;

namespace TravelEase.Application.DiscountManagement.Handlers
{
    public class GetAllDiscountsByRoomTypeQueryHandler 
        : IRequestHandler<GetAllDiscountsByRoomTypeQuery, PaginatedList<DiscountResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllDiscountsByRoomTypeQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginatedList<DiscountResponse>> Handle
            (GetAllDiscountsByRoomTypeQuery request, CancellationToken cancellationToken)
        {
            await EnsureRoomTypeExistsAsync(request.RoomTypeId);

            var paginatedList = await
                _unitOfWork.Discounts
                    .GetAllByRoomTypeIdAsync(
                        request.RoomTypeId,
                        request.PageNumber,
                        request.PageSize);

            return new PaginatedList<DiscountResponse>(
                _mapper.Map<List<DiscountResponse>>(paginatedList.Items),
                paginatedList.PageData);
        }

        private async Task EnsureRoomTypeExistsAsync(Guid hotelId)
        {
            if (!await _unitOfWork.RoomTypes.ExistsAsync(hotelId))
                throw new NotFoundException("RoomType doesn't exist.");
        }
    }
}