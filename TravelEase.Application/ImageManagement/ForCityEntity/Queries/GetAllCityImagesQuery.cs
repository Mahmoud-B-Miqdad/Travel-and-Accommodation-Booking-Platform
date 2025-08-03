using MediatR;
using TravelEase.Domain.Common.Models.PaginationModels;

namespace TravelEase.Application.ImageManagement.ForCityEntity.Queries
{
    public class GetAllCityImagesQuery : IRequest<PaginatedList<string>>
    {
        public Guid CityId { get; init; }
        public int PageNumber { get; init; } = 1;
        public int PageSize { get; init; } = 10;
    }
}