using AutoMapper;
using MediatR;
using TravelEase.Application.HotelManagement.DTOs.Responses;
using TravelEase.Application.HotelManagement.Queries;
using TravelEase.Domain.Common.Interfaces;
using TravelEase.Domain.Exceptions;

namespace TravelEase.Application.HotelManagement.Handlers
{
    public class GetHotelByIdQueryHandler : IRequestHandler<GetHotelByIdQuery, HotelWithoutRoomsResponse?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetHotelByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<HotelWithoutRoomsResponse?> Handle(GetHotelByIdQuery request, CancellationToken cancellationToken)
        {
            var hotel = await _unitOfWork.Hotels.GetByIdAsync(request.Id);
            if (hotel == null)
                throw new NotFoundException($"Hotel with Id {request.Id} was not found.");

            return _mapper.Map<HotelWithoutRoomsResponse>(hotel);
        }
    }
}