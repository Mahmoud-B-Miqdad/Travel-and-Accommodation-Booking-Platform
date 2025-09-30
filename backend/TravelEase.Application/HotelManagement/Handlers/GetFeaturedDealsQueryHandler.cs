using MediatR;
using TravelEase.Application.HotelManagement.Queries;
using TravelEase.Domain.Common.Interfaces;
using TravelEase.Domain.Common.Models.CommonModels;

namespace TravelEase.Application.HotelManagement.Handlers
{
    public class GetFeaturedDealsQueryHandler : IRequestHandler<GetFeaturedDealsQuery, List<FeaturedDeal>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetFeaturedDealsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<FeaturedDeal>> Handle
            (GetFeaturedDealsQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Hotels.GetFeaturedDealsAsync(request.Count);
        }
    }
}