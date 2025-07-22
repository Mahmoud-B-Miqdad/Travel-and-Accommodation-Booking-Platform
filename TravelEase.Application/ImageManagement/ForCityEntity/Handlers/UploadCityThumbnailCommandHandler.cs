using MediatR;
using TravelEase.Application.ImageManagement.ForCityEntity.Commands;
using TravelEase.Application.ImageManagement.Mappings;
using TravelEase.Domain.Common.Interfaces;
using TravelEase.Domain.Enums;
using TravelEase.Domain.Exceptions;

namespace TravelEase.Application.ImageManagement.ForCityEntity.Handlers
{
    public class UploadCityThumbnailCommandHandler : IRequestHandler<UploadCityThumbnailCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IImageService _imageService;
        private readonly IMediator _mediator;

        public UploadCityThumbnailCommandHandler(
            IUnitOfWork unitOfWork,
            IImageService imageService,
            IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _imageService = imageService;
            _mediator = mediator;
        }

        public async Task Handle(UploadCityThumbnailCommand request, CancellationToken cancellationToken)
        {
            await EnsureCityExists(request.CityId);

            var imageCreationDto = await ImageFormFileMapper.CreateFromFormFileAsync(
                request.CityId, request.File, ImageType.Thumbnail);

            await _imageService.UploadThumbnailAsync(imageCreationDto);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        private async Task EnsureCityExists(Guid cityId)
        {
            if (!await _unitOfWork.Hotels.ExistsAsync(cityId))
                throw new NotFoundException("City doesn't exist.");
        }
    }
}