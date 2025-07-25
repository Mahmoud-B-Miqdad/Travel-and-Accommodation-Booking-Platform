﻿using MediatR;
using Microsoft.AspNetCore.Http;

namespace TravelEase.Application.ImageManagement.ForHotelEntity.Commands
{
    public record UploadHotelThumbnailCommand : IRequest
    {
        public Guid HotelId { get; init; }
        public IFormFile File { get; init; }
    }
}