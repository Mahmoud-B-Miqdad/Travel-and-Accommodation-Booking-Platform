using MediatR;
using TravelEase.Application.ImageManagement.ForHotelEntity.Commands;
using TravelEase.Application.ImageManagement.Mappings;
using TravelEase.Domain.Common.Interfaces;
using TravelEase.Domain.Enums;
using TravelEase.Domain.Exceptions;

namespace TravelEase.Application.ImageManagement.ForHotelEntity.Handlers
{
    public class UploadHotelThumbnailCommandHandler : IRequestHandler<UploadHotelThumbnailCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IImageService _imageService;
        private readonly IMediator _mediator;

        public UploadHotelThumbnailCommandHandler(
            IUnitOfWork unitOfWork,
            IImageService imageService,
            IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _imageService = imageService;
            _mediator = mediator;
        }

        public async Task Handle(UploadHotelThumbnailCommand request, CancellationToken cancellationToken)
        {
            await EnsureHotelExists(request.HotelId);

            var imageCreationDto = await ImageFormFileMapper.CreateFromFormFileAsync(
                request.HotelId, request.File, ImageType.Thumbnail);

            await _imageService.UploadThumbnailAsync(imageCreationDto);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        private async Task EnsureHotelExists(Guid hotelId)
        {
            if (!await _unitOfWork.Hotels.ExistsAsync(hotelId))
                throw new NotFoundException("Hotel doesn't exist.");
        }
    }
}