using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TravelEase.API.Common.Responses;
using TravelEase.Application.ReviewsManagement.DTOs.Requests;
using TravelEase.Application.ReviewsManagement.DTOs.Responses;
using TravelEase.Application.ReviewsManagement.Queries;
using TravelEase.Application.RoomManagement.DTOs.Responses;
using System.Text.Json;
using TravelEase.Application.RoomManagement.Queries;
using TravelEase.Application.RoomManagement.DTOs.Requests;

namespace TravelEase.API.Controllers
{
    [Route("api/room")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public RoomController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        /// <summary>
        /// Retrieves a paginated list of rooms for a specific hotel.
        /// </summary>
        /// <param name="hotelId">The ID of the hotel for which rooms are requested.</param>
        /// <param name="roomQueryRequest">DTO containing parameters for pagination and filtering.</param>
        /// <returns>
        /// Returns a paginated list of rooms for the specified hotel.
        /// </returns>
        /// <response code="200">Returns a paginated list of rooms.</response>
        [HttpGet("~/api/hotels/{hotelId:guid}/rooms")]
        [ProducesResponseType(typeof(ApiResponse<List<RoomResponse>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<List<RoomResponse>>>> GetAllRoomsByHotelIdAsync(Guid hotelId,
            [FromQuery] RoomQueryRequest roomQueryRequest)
        {
            var roomsQuery = _mapper.Map<GetAllRoomsByHotelIdQuery>(roomQueryRequest);
            roomsQuery.HotelId = hotelId;

            var paginatedListOfRooms = await _mediator.Send(roomsQuery);
            Response.Headers.Append("X-Pagination",
                JsonSerializer.Serialize(paginatedListOfRooms.PageData));

            return Ok(ApiResponse<List<RoomResponse>>.SuccessResponse(paginatedListOfRooms.Items));
        }
    }
}