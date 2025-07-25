﻿using TravelEase.Domain.Enums;

namespace TravelEase.Domain.Aggregates.Images
{
    public class Image
    {
        public Guid Id { get; set; }
        public Guid EntityId { get; set; }
        public string Url { get; set; }
        public ImageType Type { get; set; }
        public ImageFormat Format { get; set; }
    }
}