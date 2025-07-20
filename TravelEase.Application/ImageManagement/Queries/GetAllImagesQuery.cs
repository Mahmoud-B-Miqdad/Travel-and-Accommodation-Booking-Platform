using MediatR;

namespace TravelEase.Application.ImageManagement.Queries
{
    public record GetAllImagesQuery(Guid EntityId) : IRequest<List<string>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}