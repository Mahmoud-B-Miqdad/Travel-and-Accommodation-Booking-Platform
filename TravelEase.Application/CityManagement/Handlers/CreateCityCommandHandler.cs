using AutoMapper;
using MediatR;
using TravelEase.Application.CityManagement.Commands;
using TravelEase.Application.CityManagement.DTOs.Responses;
using TravelEase.Domain.Aggregates.Cities;
using TravelEase.Domain.Common.Interfaces;
using TravelEase.Domain.Exceptions;

namespace TravelEase.Application.CityManagement.Handlers
{
    public class CreateCityCommandHandler : IRequestHandler<CreateCityCommand, CityWithoutHotelsResponse?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateCityCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CityWithoutHotelsResponse?> Handle(CreateCityCommand request, CancellationToken cancellationToken)
        {
            await EnsureCityDoesNotExistAsync(request.Name);

            var city = _mapper.Map<City>(request);
            var addedCity = await _unitOfWork.Cities.AddAsync(city);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return _mapper.Map<CityWithoutHotelsResponse>(addedCity);
        }

        private async Task EnsureCityDoesNotExistAsync(string name)
        {
            if (await _unitOfWork.Cities.ExistsAsync(name))
                throw new ConflictException($"City with name '{name}' already exists.");
        }
    }
}