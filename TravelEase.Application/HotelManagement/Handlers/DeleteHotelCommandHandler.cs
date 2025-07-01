using MediatR;
using TravelEase.Application.HotelManagement.Commands;
using TravelEase.Domain.Common.Interfaces;
using TravelEase.Domain.Exceptions;

namespace TravelEase.Application.HotelManagement.Handlers
{
    public class DeleteHotelCommandHandler : IRequestHandler<DeleteHotelCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteHotelCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(DeleteHotelCommand request, CancellationToken cancellationToken)
        {

            var existingHotel = await _unitOfWork.Hotels.GetByIdAsync(request.Id);
            if (existingHotel == null)
                throw new NotFoundException("Hotel Doesn't Exists To Delete");

            _unitOfWork.Hotels.Remove(existingHotel);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}