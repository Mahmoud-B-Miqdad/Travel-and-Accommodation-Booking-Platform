using AutoMapper;
using MediatR;
using TravelEase.Application.RoomManagement.DTOs.Responses;
using TravelEase.Application.RoomManagement.Queries;
using TravelEase.Domain.Common.Interfaces;
using TravelEase.Domain.Exceptions;

namespace TravelEase.Application.RoomManagement.Handlers
{
    public class GetRoomByIdQueryHandler : IRequestHandler<GetRoomByIdQuery, RoomResponse?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetRoomByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<RoomResponse?> Handle(GetRoomByIdQuery request, CancellationToken cancellationToken)
        {
            var room = await _unitOfWork.Rooms.GetByIdAsync(request.Id);
            if (room == null)
                throw new NotFoundException($"Room with Id {request.Id} was not found.");

            return _mapper.Map<RoomResponse>(room);
        }
    }
}