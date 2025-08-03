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
            await EnsureHotelExistsAsync(request.Id);
            await EnsureNameIsUniqueAsync(request.Name);

            var hotelToUpdate = _mapper.Map<Hotel>(request);
            _unitOfWork.Hotels.Update(hotelToUpdate);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        private async Task EnsureHotelExistsAsync(Guid hotelId)
        {
            if (!await _unitOfWork.Hotels.ExistsAsync(hotelId))
                throw new NotFoundException($"Hotel with ID {hotelId} doesn't exist to update.");
        }

        private async Task EnsureNameIsUniqueAsync(string name)
        {
            if (await _unitOfWork.Hotels.ExistsAsync(name))
                throw new ConflictException($"Hotel with name '{name}' already exists.");
        }
    }
}
