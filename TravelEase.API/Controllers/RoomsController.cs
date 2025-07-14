using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TravelEase.API.Common.Responses;
using TravelEase.Application.RoomManagement.DTOs.Responses;
using System.Text.Json;
using TravelEase.Application.RoomManagement.Queries;
using TravelEase.Application.RoomManagement.DTOs.Requests;
using TravelEase.Application.RoomManagement.Commands;
using Microsoft.AspNetCore.Authorization;

namespace TravelEase.API.Controllers
{
    [Route("api/hotels/{hotelId:guid}/rooms")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public RoomsController(IMediator mediator, IMapper mapper)
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
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<List<RoomResponse>>), StatusCodes.Status200OK)]
        [Authorize(Policy = "MustBeAdmin")]
        public async Task<ActionResult<ApiResponse<List<RoomResponse>>>> GetAllRoomsByHotelIdAsync(Guid hotelId,
            [FromQuery] RoomQueryRequest roomQueryRequest)
        {
            var roomsQuery = _mapper.Map<GetAllRoomsByHotelIdQuery>(roomQueryRequest);
            roomsQuery.HotelId = hotelId;

            var paginatedListOfRooms = await _mediator.Send(roomsQuery);
            Response.Headers.Append("X-Pagination",
                JsonSerializer.Serialize(paginatedListOfRooms.PageData));

            var response = ApiResponse<List<RoomResponse>>.SuccessResponse(paginatedListOfRooms.Items);
            return Ok(response);
        }

        /// <summary>
        /// Gets a specific room by its ID within a specific hotel.
        /// </summary>
        /// <param name="hotelId">Hotel ID.</param>
        /// <param name="roomId">Room ID.</param>
        /// <returns>Returns the room details if found; otherwise, NotFound.</returns>
        /// <response code="200">Returns the room details.</response>
        [HttpGet("{roomId:guid}", Name = "GetRoomByIdAndHotelIdAsync")]
        [ProducesResponseType(typeof(ApiResponse<RoomResponse>), StatusCodes.Status200OK)]
        [Authorize(Policy = "MustBeAdmin")]
        public async Task<ActionResult<ApiResponse<RoomResponse>>> 
            GetRoomByIdAndHotelIdAsync(Guid hotelId, Guid roomId)
        {
            var request = new GetRoomByIdAndHotelIdQuery
            {
                HotelId = hotelId,
                RoomId = roomId
            };
            var result = await _mediator.Send(request);

            var response = ApiResponse<RoomResponse>.SuccessResponse(result);
            return Ok(response);
        }

        /// <summary>
        /// Creates a new room based on the provided data.
        /// </summary>
        /// <param name="roomRequest">The data for creating a new room.</param>
        /// <param name="hotelId">The unique identifier of the hotel.</param>
        /// <returns>Returns the created room details.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<RoomResponse>), StatusCodes.Status201Created)]
        [Authorize(Policy = "AdminOrOwner")]
        public async Task<ActionResult<ApiResponse<RoomResponse>>>
            CreateRoomForHotelAsync(RoomForCreationRequest roomRequest, Guid hotelId)
        {
            var request = _mapper.Map<CreateRoomCommand>(roomRequest);
            request.HotelId = hotelId;
            var roomToReturn = await _mediator.Send(request);

            var response = ApiResponse<RoomResponse>.SuccessResponse(roomToReturn,
                "Room created successfully");

            return CreatedAtAction("GetRoomByIdAndHotelIdAsync",
            new
            {
                hotelId,
                roomId = roomToReturn.Id
            }, response);
        }

        /// <summary>
        /// Retrieves available rooms for a specific hotel based on check-in and check-out dates.
        /// </summary>
        /// <param name="hotelId">The unique identifier of the hotel.</param>
        /// <param name="hotelAvailableRoomsRequest">Check-in and check-out dates to filter availability.</param>
        /// <returns>
        /// 200 OK with a list of available rooms wrapped in an ApiResponse.
        /// </returns>
        [HttpGet("available")]
        [ProducesResponseType(typeof(ApiResponse<List<RoomResponse>>), StatusCodes.Status200OK)]
        [Authorize]
        public async Task<ActionResult<ApiResponse<List<RoomResponse>>>> GetHotelAvailableRoomsAsync(
            Guid hotelId,
            [FromQuery] GetHotelAvailableRoomsRequest hotelAvailableRoomsRequest)
        {
            var request = _mapper.Map<GetHotelAvailableRoomsQuery>(hotelAvailableRoomsRequest);
            request.HotelId = hotelId;

            var rooms = await _mediator.Send(request);
            var response = ApiResponse<List<RoomResponse>>.SuccessResponse(rooms);

            return Ok(response);
        }
    }
}