using AutoMapper;
using MediatR;
using TravelEase.Application.HotelManagement.DTOs.Responses;
using TravelEase.Application.HotelManagement.Queries;
using TravelEase.Domain.Aggregates.Hotels;
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

        public async Task<HotelWithoutRoomsResponse?> Handle
            (GetHotelByIdQuery request, CancellationToken cancellationToken)
        {
            var hotel = await GetHotelOrThrowAsync(request.Id);
            return _mapper.Map<HotelWithoutRoomsResponse>(hotel);
        }

        private async Task<Hotel> GetHotelOrThrowAsync(Guid hotelId)
        {
            var hotel = await _unitOfWork.Hotels.GetByIdAsync(hotelId);
            if (hotel == null)
                throw new NotFoundException($"Hotel with Id {hotelId} was not found.");
            return hotel;
        }
    }
}