using MediatR;
using TravelEase.Application.ImageManagement.ForCityEntity.Queries;
using TravelEase.Domain.Common.Interfaces;
using TravelEase.Domain.Common.Models.PaginationModels;
using TravelEase.Domain.Exceptions;

namespace TravelEase.Application.ImageManagement.ForCityEntity.Handlers
{
    public class GetAllCityImagesQueryHandler : IRequestHandler<GetAllCityImagesQuery, PaginatedList<string>>
    {
        private readonly IImageService _imageService;
        private readonly IUnitOfWork _unitOfWork;

        public GetAllCityImagesQueryHandler(IImageService imageService, IUnitOfWork unitOfWork)
        {
            _imageService = imageService;
            _unitOfWork = unitOfWork;
        }

        public async Task<PaginatedList<string>> Handle
            (GetAllCityImagesQuery request, CancellationToken cancellationToken)
        {
            await EnsureCityExists(request.CityId);

            var imageUrls = await _imageService.GetAllImagesAsync
                (request.CityId, request.PageNumber, request.PageSize);

            return imageUrls;
        }

        private async Task EnsureCityExists(Guid hotelId)
        {
            if (!await _unitOfWork.Cities.ExistsAsync(hotelId))
                throw new NotFoundException("City doesn't exist.");
        }
    }
}