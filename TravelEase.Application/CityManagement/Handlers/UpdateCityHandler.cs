using AutoMapper;
using MediatR;

using TravelEase.Application.CityManagement.Commands;
using TravelEase.Domain.Aggregates.Cities;
using TravelEase.Domain.Common.Interfaces;
using TravelEase.Domain.Exceptions;

namespace TravelEase.Application.CityManagement.Handlers
{
    public class UpdateCityHandler : IRequestHandler<UpdateCityCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateCityHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task Handle(UpdateCityCommand request, CancellationToken cancellationToken)
        {
            var existingCity =  await _unitOfWork.Cities.GetByIdAsync(request.Id);
            
            if(existingCity == null)
                throw new NotFoundException($"City With {request.Id} Doesn't Exists To UpdateAsync");

            var conflictingCity = await _unitOfWork.Cities.IsExistsAsync(request.Name);
            if (conflictingCity)
                throw new ConflictException($"Another city with name '{request.Name}' already exists.");


            var cityToUpdate = _mapper.Map<City>(request);
            _unitOfWork.Cities.Update(cityToUpdate);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}