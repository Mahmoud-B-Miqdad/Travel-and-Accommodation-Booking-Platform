using AutoMapper;
using MediatR;
using TravelEase.Application.CityManagement.DTOs.Responses;
using TravelEase.Application.CityManagement.Queries;
using TravelEase.Domain.Common.Interfaces;

namespace TravelEase.Application.CityManagement.Handlers
{
    public class GetTrendingCitiesQueryHandler
            : IRequestHandler<GetTrendingCitiesQuery, List<CityWithoutHotelsResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetTrendingCitiesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<CityWithoutHotelsResponse>> Handle(
            GetTrendingCitiesQuery request, CancellationToken cancellationToken)
        {
            var cities = await _unitOfWork.Cities.GetTrendingCitiesAsync(request.Count);
            return _mapper.Map<List<CityWithoutHotelsResponse>>(cities);
        }
    }
}