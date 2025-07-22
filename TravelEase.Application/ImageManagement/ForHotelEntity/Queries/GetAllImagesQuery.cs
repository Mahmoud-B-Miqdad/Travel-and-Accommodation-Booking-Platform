using MediatR;
using TravelEase.Domain.Common.Models.PaginationModels;

namespace TravelEase.Application.ImageManagement.ForHotelEntity.Queries
{
    public class GetAllImagesQuery : IRequest<PaginatedList<string>>
    {
        public Guid HotelId { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}