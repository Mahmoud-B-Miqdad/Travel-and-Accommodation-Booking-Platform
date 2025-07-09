using AutoMapper;
using MediatR;
using TravelEase.Application.HotelManagement.Commands;
using TravelEase.Application.HotelManagement.DTOs.Responses;
using TravelEase.Domain.Aggregates.Hotels;
using TravelEase.Domain.Common.Interfaces;
using TravelEase.Domain.Exceptions;

namespace TravelEase.Application.HotelManagement.Handlers
{
    public class CreateHotelCommandHandler : IRequestHandler<CreateHotelCommand, HotelWithoutRoomsResponse?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateHotelCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<HotelWithoutRoomsResponse?> Handle(CreateHotelCommand request, CancellationToken cancellationToken)
        {
            await EnsureHotelDoesNotExistAsync(request.Name);

            var hotel = _mapper.Map<Hotel>(request);
            var addedHotel = await _unitOfWork.Hotels.AddAsync(hotel);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return _mapper.Map<HotelWithoutRoomsResponse>(addedHotel);
        }

        private async Task EnsureHotelDoesNotExistAsync(string name)
        {
            if (await _unitOfWork.Hotels.ExistsAsync(name))
                throw new ConflictException($"Hotel with name '{name}' already exists.");
        }
    }
}