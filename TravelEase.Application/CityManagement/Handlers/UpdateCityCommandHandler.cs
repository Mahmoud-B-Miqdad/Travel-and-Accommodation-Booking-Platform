using AutoMapper;
using MediatR;
using TravelEase.Application.CityManagement.Commands;
using TravelEase.Domain.Aggregates.Cities;
using TravelEase.Domain.Common.Interfaces;
using TravelEase.Domain.Exceptions;

namespace TravelEase.Application.CityManagement.Handlers
{
    public class UpdateCityCommandHandler : IRequestHandler<UpdateCityCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateCityCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task Handle(UpdateCityCommand request, CancellationToken cancellationToken)
        {
            await EnsureCityExistsAsync(request.Id);
            await EnsureNameIsUniqueAsync(request.Name);

            var cityToUpdate = _mapper.Map<City>(request);
            _unitOfWork.Cities.Update(cityToUpdate);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        private async Task EnsureCityExistsAsync(Guid cityId)
        {
            var exists = await _unitOfWork.Cities.ExistsAsync(cityId);
            if (!exists)
                throw new NotFoundException($"City with ID {cityId} doesn't exist to update.");
        }

        private async Task EnsureNameIsUniqueAsync(string name)
        {
            var nameExists = await _unitOfWork.Cities.ExistsAsync(name);
            if (nameExists)
                throw new ConflictException($"Another city with name '{name}' already exists.");
        }
    }
}