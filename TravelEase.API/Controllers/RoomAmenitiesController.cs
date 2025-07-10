using AutoMapper;
using MediatR;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using TravelEase.API.Common.Responses;
using TravelEase.Application.RoomAmenityManagement.Query;
using TravelEase.Application.RoomAmenityManagement.DTOs.Responses;
using TravelEase.Application.RoomAmenityManagement.Commands;
using TravelEase.Application.RoomAmenityManagement.DTOs.Requests;
using Microsoft.AspNetCore.Authorization;

namespace TravelEase.API.Controllers
{
    [Route("api/room-amenities")]
    [ApiController]
    [ApiVersion("1.0")]
    public class RoomAmenitiesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public RoomAmenitiesController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        /// <summary>
        /// Retrieves a paginated list of room amenities based on the specified search criteria.
        /// </summary>
        /// <param name="getAllRoomAmenitiesQuery">Query parameters for retrieving room amenities.</param>
        /// <returns>Returns a paginated list of room amenities.</returns>
        /// <remarks>
        /// This endpoint supports pagination to retrieve a subset of room amenities based on the provided search.
        /// </remarks>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<List<RoomAmenityResponse>>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<List<RoomAmenityResponse>>>> GetAllRoomAmenitiesAsync(
            [FromQuery] GetAllRoomAmenitiesQuery getAllRoomAmenitiesQuery)
        {
            var paginatedListOfAmenities = await _mediator.Send(getAllRoomAmenitiesQuery);
            Response.Headers.Append("X-Pagination",
                JsonSerializer.Serialize(paginatedListOfAmenities.PageData));

            var response = ApiResponse<List<RoomAmenityResponse>>.SuccessResponse(paginatedListOfAmenities.Items);
            return Ok(response);
        }

        /// <summary>
        /// Retrieves details for a specific room amenity.
        /// </summary>
        /// <param name="roomAmenityId">The unique identifier for the room amenity.</param>
        /// <returns>Returns the room amenity details.</returns>
        [HttpGet("{roomAmenityId:guid}", Name = "GetRoomAmenity")]
        [ProducesResponseType(typeof(ApiResponse<RoomAmenityResponse>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<RoomAmenityResponse>>> GetRoomAmenityAsync(Guid roomAmenityId)
        {
            var request = new GetRoomAmenityByIdQuery { Id = roomAmenityId };
            var roomAmenity = await _mediator.Send(request);

            var response = ApiResponse<RoomAmenityResponse>.SuccessResponse(roomAmenity!);
            return Ok(response);
        }

        /// <summary>
        /// Creates a new room amenity based on the provided data.
        /// </summary>
        /// <param name="roomAmenityRequest">The data for creating a new room amenity.</param>
        /// <returns>Returns the created room amenity details.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<RoomAmenityResponse>), StatusCodes.Status201Created)]
        [Authorize(Policy = "MustBeAdmin")]
        public async Task<ActionResult<ApiResponse<RoomAmenityResponse>>>
            CreateRoomAmenityAsync(RoomAmenityForCreationRequest roomAmenityRequest)
        {
            var request = _mapper.Map<CreateRoomAmenityCommand>(roomAmenityRequest);
            var amenityToReturn = await _mediator.Send(request);

            var response = ApiResponse<RoomAmenityResponse>.SuccessResponse(amenityToReturn,
                "Room Amenity created successfully");

            return CreatedAtRoute("GetRoomAmenity",
            new
            {
                roomAmenityId = amenityToReturn.Id
            }, response);
        }

        /// <summary>
        /// Updates an existing room amenity with the provided data.
        /// </summary>
        /// <param name="roomAmenityId">The unique identifier for the room amenity.</param>
        /// <param name="roomAmenityForUpdateRequest">The data for updating the room amenity.</param>
        /// <returns>Indicates successful update.</returns>
        [HttpPut("{roomAmenityId:guid}")]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        [Authorize(Policy = "MustBeAdmin")]
        public async Task<ActionResult<ApiResponse<string>>> UpdateRoomAmenity(Guid roomAmenityId,
        RoomAmenityForUpdateRequest roomAmenityForUpdateRequest)
        {
            var request = _mapper.Map<UpdateRoomAmenityCommand>(roomAmenityForUpdateRequest);
            request.Id = roomAmenityId;
            await _mediator.Send(request);

            var response = ApiResponse<string>.SuccessResponse(null, "Room Amenity updated successfully.");
            return Ok(response);
        }

        /// <summary>
        /// Deletes a room amenity with the specified ID.
        /// </summary>
        /// <param name="roomAmenityId">The unique identifier for the room amenity.</param>
        /// <returns>Indicates successful deletion.</returns>ss
        [HttpDelete("{roomAmenityId:guid}")]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        [Authorize(Policy = "MustBeAdmin")]
        public async Task<ActionResult<ApiResponse<string>>> DeleteRoomAmenity(Guid roomAmenityId)
        {
            var deleteRoomAmenityCommand = new DeleteRoomAmenityCommand { Id = roomAmenityId };
            await _mediator.Send(deleteRoomAmenityCommand);

            var response = ApiResponse<string>.SuccessResponse(null, "Room Amenity deleted successfully.");
            return Ok(response);
        }
    }
}