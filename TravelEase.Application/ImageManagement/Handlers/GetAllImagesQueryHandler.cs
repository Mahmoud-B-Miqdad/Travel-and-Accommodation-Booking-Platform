using MediatR;
using TravelEase.Application.ImageManagement.Queries;
using TravelEase.Domain.Aggregates.Hotels;
using TravelEase.Domain.Common.Interfaces;
using TravelEase.Domain.Common.Models.PaginationModels;
using TravelEase.Domain.Exceptions;

namespace TravelEase.Application.ImageManagement.Handlers
{
    public class GetAllImagesQueryHandler : IRequestHandler<GetAllImagesQuery, PaginatedList<string>>
    {
        private readonly IImageService _imageService;
        private readonly IUnitOfWork _unitOfWork;

        public GetAllImagesQueryHandler(IImageService imageService, IUnitOfWork unitOfWork)
        {
            _imageService = imageService;
            _unitOfWork = unitOfWork;
        }

        public async Task<PaginatedList<string>> Handle
            (GetAllImagesQuery request, CancellationToken cancellationToken)
        {
            await EnsureHotelExists(request.HotelId);

            var imageUrls = await _imageService.GetAllImagesAsync
                (request.HotelId, request.PageNumber, request.PageSize);

            return imageUrls;
        }

        private async Task EnsureHotelExists(Guid hotelId)
        {
            if (!await _unitOfWork.Hotels.ExistsAsync(hotelId))
                throw new NotFoundException("Hotel doesn't exist.");
        }
    }
}