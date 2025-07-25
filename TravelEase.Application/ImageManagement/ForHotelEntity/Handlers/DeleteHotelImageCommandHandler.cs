﻿using MediatR;
using TravelEase.Application.ImageManagement.ForHotelEntity.Commands;
using TravelEase.Domain.Common.Interfaces;
using TravelEase.Domain.Exceptions;

namespace TravelEase.Application.ImageManagement.ForHotelEntity.Handlers
{
    public class DeleteHotelImageCommandHandler : IRequestHandler<DeleteHotelImageCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IImageService _imageService;

        public DeleteHotelImageCommandHandler(IUnitOfWork unitOfWork, IImageService imageService)
        {
            _unitOfWork = unitOfWork;
            _imageService = imageService;
        }

        public async Task Handle(DeleteHotelImageCommand request, CancellationToken cancellationToken)
        {
            await EnsureHotelExists(request.HotelId);

            await _imageService.DeleteImageAsync(request.HotelId, request.ImageId);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        private async Task EnsureHotelExists(Guid hotelId)
        {
            if (!await _unitOfWork.Hotels.ExistsAsync(hotelId))
                throw new NotFoundException("Hotel doesn't exist.");
        }
    }
}