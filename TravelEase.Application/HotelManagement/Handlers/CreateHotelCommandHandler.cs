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
            var existingHotel = await _unitOfWork.Hotels.IsExistsAsync(request.Name);
            if (existingHotel)
                throw new ConflictException($"Hotel with name '{request.Name}' already exists.");

            var hotelToAdd = _mapper.Map<Hotel>(request);
            var addedHotel = await _unitOfWork.Hotels.AddAsync(hotelToAdd);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return _mapper.Map<HotelWithoutRoomsResponse>(addedHotel);
        }
    }
}