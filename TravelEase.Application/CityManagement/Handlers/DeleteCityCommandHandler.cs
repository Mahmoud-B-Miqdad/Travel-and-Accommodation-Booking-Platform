using MediatR;
using TravelEase.Application.CityManagement.Commands;
using TravelEase.Domain.Aggregates.Cities;
using TravelEase.Domain.Common.Interfaces;
using TravelEase.Domain.Exceptions;

namespace TravelEase.Application.CityManagement.Handlers
{
    public class DeleteCityCommandHandler : IRequestHandler<DeleteCityCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteCityCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(DeleteCityCommand request, CancellationToken cancellationToken)
        {
            var city = await GetExistingCityAsync(request.Id);
            _unitOfWork.Cities.Remove(city);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        private async Task<City> GetExistingCityAsync(Guid cityId)
        {
            var city = await _unitOfWork.Cities.GetByIdAsync(cityId);
            if (city == null)
                throw new NotFoundException("City doesn't exist to delete.");
            return city;
        }
    }
}