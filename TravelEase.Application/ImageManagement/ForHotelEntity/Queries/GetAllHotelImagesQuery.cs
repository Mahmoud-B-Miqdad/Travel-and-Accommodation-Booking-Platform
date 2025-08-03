using MediatR;
using TravelEase.Domain.Common.Models.PaginationModels;

namespace TravelEase.Application.ImageManagement.ForHotelEntity.Queries
{
    public class GetAllHotelImagesQuery : IRequest<PaginatedList<string>>
    {
        public Guid HotelId { get; init; }
        public int PageNumber { get; init; } = 1;
        public int PageSize { get; init; } = 10;
    }
}