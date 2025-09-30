using MediatR;
using TravelEase.Application.ImageManagement.ForCityEntity.Commands;
using TravelEase.Application.ImageManagement.Mappings;
using TravelEase.Domain.Common.Interfaces;
using TravelEase.Domain.Enums;
using TravelEase.Domain.Exceptions;

namespace TravelEase.Application.ImageManagement.ForCityEntity.Handlers
{
    public class UploadCityImageCommandHandler : IRequestHandler<UploadCityImageCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IImageService _imageService;

        public UploadCityImageCommandHandler(IUnitOfWork unitOfWork, IImageService imageService)
        {
            _unitOfWork = unitOfWork;
            _imageService = imageService;
        }

        public async Task Handle(UploadCityImageCommand request, CancellationToken cancellationToken)
        {
            await EnsureCityExists(request.CityId);

            var imageCreationDto = await ImageFormFileMapper.CreateFromFormFileAsync(
                request.CityId, request.File, ImageType.Gallery);

            await _imageService.UploadImageAsync(imageCreationDto);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        private async Task EnsureCityExists(Guid cityId)
        {
            if (!await _unitOfWork.Cities.ExistsAsync(cityId))
                throw new NotFoundException("City doesn't exist.");
        }
    }
}