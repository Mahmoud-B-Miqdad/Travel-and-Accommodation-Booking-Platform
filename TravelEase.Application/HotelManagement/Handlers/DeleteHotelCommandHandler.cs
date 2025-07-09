using MediatR;
using TravelEase.Application.HotelManagement.Commands;
using TravelEase.Domain.Aggregates.Hotels;
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
            var hotel = await GetHotelOrThrowAsync(request.Id);
            _unitOfWork.Hotels.Remove(hotel);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        private async Task<Hotel> GetHotelOrThrowAsync(Guid hotelId)
        {
            var hotel = await _unitOfWork.Hotels.GetByIdAsync(hotelId);
            if (hotel == null)
                throw new NotFoundException("Hotel doesn't exist to delete.");
            return hotel;
        }
    }
}