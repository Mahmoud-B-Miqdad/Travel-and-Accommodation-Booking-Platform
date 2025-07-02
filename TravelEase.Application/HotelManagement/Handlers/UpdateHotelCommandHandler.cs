using AutoMapper;
using MediatR;
using TravelEase.Application.HotelManagement.Commands;
using TravelEase.Domain.Aggregates.Hotels;
using TravelEase.Domain.Common.Interfaces;
using TravelEase.Domain.Exceptions;

namespace TravelEase.Application.HotelManagement.Handlers
{
    public class UpdateHotelCommandHandler : IRequestHandler<UpdateHotelCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateHotelCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task Handle(UpdateHotelCommand request, CancellationToken cancellationToken)
        {
            var existingHotel = await _unitOfWork.Hotels.ExistsAsync(request.Id);
            if (!existingHotel)
                throw new NotFoundException($"Hotel With {request.Id} Doesn't Exists To Update");

            var conflictingHotel = await _unitOfWork.Hotels.ExistsAsync(request.Name);
            if (conflictingHotel)
                throw new ConflictException($"Another hotel with name '{request.Name}' already exists.");

            var hotelToUpdate = _mapper.Map<Hotel>(request);
            _unitOfWork.Hotels.Update(hotelToUpdate);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
