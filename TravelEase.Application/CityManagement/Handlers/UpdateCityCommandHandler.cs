﻿using AutoMapper;
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
            var existingCity =  await _unitOfWork.Cities.ExistsAsync(request.Id);
            if(!existingCity)
                throw new NotFoundException($"City With {request.Id} Doesn't Exists To Update");

            var conflictingCity = await _unitOfWork.Cities.ExistsAsync(request.Name);
            if (conflictingCity)
                throw new ConflictException($"Another city with name '{request.Name}' already exists.");


            var cityToUpdate = _mapper.Map<City>(request);
            _unitOfWork.Cities.Update(cityToUpdate);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}