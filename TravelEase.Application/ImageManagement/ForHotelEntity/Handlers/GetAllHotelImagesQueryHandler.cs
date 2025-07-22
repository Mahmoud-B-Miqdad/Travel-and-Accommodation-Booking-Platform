using MediatR;
using TravelEase.Application.ImageManagement.ForHotelEntity.Queries;
using TravelEase.Domain.Common.Interfaces;
using TravelEase.Domain.Common.Models.PaginationModels;
using TravelEase.Domain.Exceptions;

namespace TravelEase.Application.ImageManagement.ForHotelEntity.Handlers
{
    public class GetAllHotelImagesQueryHandler : IRequestHandler<GetAllHotelImagesQuery, PaginatedList<string>>
    {
        private readonly IImageService _imageService;
        private readonly IUnitOfWork _unitOfWork;

        public GetAllHotelImagesQueryHandler(IImageService imageService, IUnitOfWork unitOfWork)
        {
            _imageService = imageService;
            _unitOfWork = unitOfWork;
        }

        public async Task<PaginatedList<string>> Handle
            (GetAllHotelImagesQuery request, CancellationToken cancellationToken)
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