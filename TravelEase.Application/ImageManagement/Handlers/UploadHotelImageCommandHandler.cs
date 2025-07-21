using MediatR;
using TravelEase.Application.ImageManagement.Commands;
using TravelEase.Application.ImageManagement.Mappings;
using TravelEase.Domain.Common.Interfaces;
using TravelEase.Domain.Enums;
using TravelEase.Domain.Exceptions;

namespace TravelEase.Application.ImageManagement.Handlers
{
    public class UploadHotelImageCommandHandler : IRequestHandler<UploadHotelImageCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IImageService _imageService;

        public UploadHotelImageCommandHandler(IUnitOfWork unitOfWork, IImageService imageService)
        {
            _unitOfWork = unitOfWork;
            _imageService = imageService;
        }

        public async Task Handle(UploadHotelImageCommand request, CancellationToken cancellationToken)
        {
            await EnsureHotelExists(request.HotelId);

            var imageCreationDto = await ImageFormFileMapper.FromFormFileAsync(
                request.HotelId, request.File, ImageType.Gallery);

            await _imageService.UploadImageAsync(imageCreationDto);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        private async Task EnsureHotelExists(Guid hotelId)
        {
            if (!await _unitOfWork.Hotels.ExistsAsync(hotelId))
                throw new NotFoundException("Hotel doesn't exist.");
        }
    }
}