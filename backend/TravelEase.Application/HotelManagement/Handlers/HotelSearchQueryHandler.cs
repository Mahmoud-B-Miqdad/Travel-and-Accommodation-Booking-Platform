using AutoMapper;
using MediatR;
using TravelEase.Application.HotelManagement.Queries;
using TravelEase.Domain.Common.Interfaces;
using TravelEase.Domain.Common.Models.HotelSearchModels;
using TravelEase.Domain.Common.Models.PaginationModels;

namespace TravelEase.Application.HotelManagement.Handlers
{
    public class HotelSearchQueryHandler : IRequestHandler<HotelSearchQuery, PaginatedList<HotelSearchResult>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public HotelSearchQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginatedList<HotelSearchResult>> Handle(HotelSearchQuery request,
        CancellationToken cancellationToken)
        {
            var hotelSearchParameters = _mapper.Map<HotelSearchParameters>(request);
            return await _unitOfWork.Hotels.HotelSearchAsync(hotelSearchParameters);
        }
    }
}