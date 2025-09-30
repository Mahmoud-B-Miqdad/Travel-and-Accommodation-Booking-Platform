using AutoMapper;
using MediatR;
using TravelEase.Application.CityManagement.DTOs.Responses;
using TravelEase.Application.CityManagement.Queries;
using TravelEase.Domain.Aggregates.Cities;
using TravelEase.Domain.Common.Interfaces;
using TravelEase.Domain.Exceptions;

namespace TravelEase.Application.CityManagement.Handlers
{
    public class GetCityByIdQueryHandler : IRequestHandler<GetCityByIdQuery, CityResponse?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetCityByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CityResponse?> Handle(GetCityByIdQuery request, CancellationToken cancellationToken)
        {
            var city = await GetCityOrThrowAsync(request.Id);
            return _mapper.Map<CityResponse>(city);
        }

        private async Task<City> GetCityOrThrowAsync(Guid cityId)
        {
            var city = await _unitOfWork.Cities.GetByIdAsync(cityId);
            if (city == null)
                throw new NotFoundException($"City with Id {cityId} was not found.");
            return city;
        }
    }
}