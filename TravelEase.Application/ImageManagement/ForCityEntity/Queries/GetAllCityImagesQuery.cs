using MediatR;
using TravelEase.Domain.Common.Models.PaginationModels;

namespace TravelEase.Application.ImageManagement.ForCityEntity.Queries
{
    public class GetAllCityImagesQuery : IRequest<PaginatedList<string>>
    {
        public Guid CityId { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}