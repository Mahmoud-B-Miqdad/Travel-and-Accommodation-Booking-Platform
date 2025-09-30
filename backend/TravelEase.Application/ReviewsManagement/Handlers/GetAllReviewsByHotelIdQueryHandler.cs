using AutoMapper;
using MediatR;
using TravelEase.Application.ReviewsManagement.DTOs.Responses;
using TravelEase.Application.ReviewsManagement.Queries;
using TravelEase.Domain.Common.Interfaces;
using TravelEase.Domain.Common.Models.PaginationModels;
using TravelEase.Domain.Exceptions;

namespace TravelEase.Application.ReviewsManagement.Handlers
{
    public class GetAllReviewsByHotelIdQueryHandler : 
        IRequestHandler<GetAllReviewsByHotelIdQuery, PaginatedList<ReviewResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllReviewsByHotelIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginatedList<ReviewResponse>> Handle
            (GetAllReviewsByHotelIdQuery request,
            CancellationToken cancellationToken)
        {
            await EnsureHotelExistsAsync(request.HotelId);

            var paginatedList = await
                _unitOfWork.Reviews
                    .GetAllByHotelIdAsync(
                        request.HotelId,
                        request.SearchQuery,
                        request.PageNumber,
                        request.PageSize);

            return new PaginatedList<ReviewResponse>(
                _mapper.Map<List<ReviewResponse>>(paginatedList.Items),
                paginatedList.PageData);
        }

        private async Task EnsureHotelExistsAsync(Guid hotelId)
        {
            if (!await _unitOfWork.Hotels.ExistsAsync(hotelId))
                throw new NotFoundException("Hotel doesn't exist.");
        }
    }
}