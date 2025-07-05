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
using TravelEase.Application.RoomAmenityManagement.Commands;
using TravelEase.Application.RoomAmenityManagement.DTOs.Requests;
using TravelEase.Application.RoomAmenityManagement.DTOs.Responses;
using TravelEase.Application.RoomManagement.Commands;

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

        /// <summary>
        /// Retrieves a specific room by its unique identifier.
        /// </summary>
        /// <param name="roomId">The unique identifier of the room.</param>
        /// <returns>The details of the requested room.</returns>
        [HttpGet("{roomId:guid}", Name = "GetRoom")]
        [ProducesResponseType(typeof(ApiResponse<RoomResponse>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<RoomResponse>>> GetRoomAsync(Guid roomId)
        {
            var request = new GetRoomByIdQuery { Id = roomId };
            var result = await _mediator.Send(request);
            var roomResponse = _mapper.Map<RoomResponse>(result);

            var response = ApiResponse<RoomResponse>.SuccessResponse(roomResponse);
            return Ok(response);
        }

        /// <summary>
        /// Gets a specific room by its ID within a specific hotel.
        /// </summary>
        /// <param name="hotelId">Hotel ID.</param>
        /// <param name="roomId">Room ID.</param>
        /// <returns>Returns the room details if found; otherwise, NotFound.</returns>
        /// <response code="200">Returns the room details.</response>
        [HttpGet("~/api/hotels/{hotelId:guid}/rooms/{roomId:guid}")]
        [ProducesResponseType(typeof(ApiResponse<RoomResponse>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<RoomResponse>>> 
            GetRoomByIdAndHotelIdAsync(Guid hotelId, Guid roomId)
        {
            var request = new GetRoomByIdAndHotelIdQuery
            {
                HotelId = hotelId,
                RoomId = roomId
            };
            var result = await _mediator.Send(request);
            var roomResponse = _mapper.Map<RoomResponse>(result);

            var response = ApiResponse<RoomResponse>.SuccessResponse(roomResponse);
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
        public async Task<ActionResult<ApiResponse<RoomResponse>>>
            CreateRoomForHotelAsync(RoomForCreationRequest roomRequest, Guid hotelId)
        {
            var request = _mapper.Map<CreateRoomCommand>(roomRequest);
            request.HotelId = hotelId;
            var roomToReturn = await _mediator.Send(request);

            var response = ApiResponse<RoomResponse>.SuccessResponse(roomToReturn,
                "Room created successfully");

            return CreatedAtRoute("GetRoom",
            new
            {
                roomId = roomToReturn.Id
            }, response);
        }
    }
}