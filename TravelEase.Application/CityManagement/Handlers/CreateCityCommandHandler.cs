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
            var existingCity = await _unitOfWork.Cities.ExistsAsync(request.Name);
            if (existingCity)
                throw new ConflictException($"City with name '{request.Name}' already exists.");

            var cityToAdd = _mapper.Map<City>(request);
            var addedCity = await _unitOfWork.Cities.AddAsync(cityToAdd);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return _mapper.Map<CityWithoutHotelsResponse>(addedCity);
        }
    }
}