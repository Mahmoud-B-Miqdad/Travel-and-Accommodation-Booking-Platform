using MediatR;
using TravelEase.Application.CityManagement.Commands;
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
            var existingCity = await _unitOfWork.Cities.GetByIdAsync(request.Id);
            if (existingCity == null)
                throw new NotFoundException("City Doesn't Exists To Delete");

            _unitOfWork.Cities.Remove(existingCity);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}