using MediatR;
using TravelEase.Domain.Common.Models.CommonModels;

namespace TravelEase.Application.HotelManagement.Queries
{
    public record GetFeaturedDealsQuery : IRequest<List<FeaturedDeal>>
    {
        public int Count { get; init; } = 5;
    }
}