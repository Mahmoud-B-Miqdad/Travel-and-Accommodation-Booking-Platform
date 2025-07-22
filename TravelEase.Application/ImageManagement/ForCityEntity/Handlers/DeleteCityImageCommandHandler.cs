using MediatR;
using TravelEase.Application.ImageManagement.ForCityEntity.Commands;
using TravelEase.Domain.Common.Interfaces;
using TravelEase.Domain.Exceptions;

namespace TravelEase.Application.ImageManagement.ForCityEntity.Handlers
{
    public class DeleteCityImageCommandHandler : IRequestHandler<DeleteCityImageCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IImageService _imageService;

        public DeleteCityImageCommandHandler(IUnitOfWork unitOfWork, IImageService imageService)
        {
            _unitOfWork = unitOfWork;
            _imageService = imageService;
        }

        public async Task Handle(DeleteCityImageCommand request, CancellationToken cancellationToken)
        {
            await EnsureCityExists(request.CityId);

            await _imageService.DeleteImageAsync(request.CityId, request.ImageId);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        private async Task EnsureCityExists(Guid cityId)
        {
            if (!await _unitOfWork.Cities.ExistsAsync(cityId))
                throw new NotFoundException("City doesn't exist.");
        }
    }
}