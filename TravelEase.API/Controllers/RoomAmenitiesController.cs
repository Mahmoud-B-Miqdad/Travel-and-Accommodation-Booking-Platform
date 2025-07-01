using AutoMapper;
using MediatR;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using TravelEase.API.Common.Responses;
using TravelEase.Application.RoomAmenityManagement.Query;
using TravelEase.Application.RoomAmenityManagement.DTOs.Responses;
using TravelEase.Application.HotelManagement.DTOs.Responses;
using TravelEase.Application.HotelManagement.Queries;
using Microsoft.AspNetCore.Authorization;
using TravelEase.Application.RoomAmenityManagement.Commands;
using TravelEase.Application.RoomAmenityManagement.DTOs.Requests;
using TravelEase.Domain.Exceptions;
using TravelEase.Application.HotelManagement.Commands;
using TravelEase.Domain.Aggregates.Hotels;

namespace TravelEase.API.Controllers
{
    [Route("api/[controller]")]
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllRoomAmenitiesAsync(
            [FromQuery] GetAllRoomAmenitiesQuery getAllRoomAmenitiesQuery)
        {
            var paginatedListOfAmenities = await _mediator.Send(getAllRoomAmenitiesQuery);
            Response.Headers.Append("X-Pagination",
                JsonSerializer.Serialize(paginatedListOfAmenities.PageData));

            return Ok(ApiResponse<List<RoomAmenityResponse>>.SuccessResponse(paginatedListOfAmenities.Items));
        }
        /// <summary>
        /// Retrieves details for a specific room amenity.
        /// </summary>
        /// <param name="roomAmenityId">The unique identifier for the room amenity.</param>
        /// <returns>Returns the room amenity details.</returns>
        [HttpGet("{roomAmenityId:guid}", Name = "GetRoomAmenity")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetRoomAmenityAsync(Guid roomAmenityId)
        {
            var request = new GetRoomAmenityByIdQuery { Id = roomAmenityId };
            var roomAmenity = await _mediator.Send(request);

            return Ok(ApiResponse<RoomAmenityResponse>.SuccessResponse(roomAmenity!));
        }

        /// <summary>
        /// Creates a new room amenity based on the provided data.
        /// </summary>
        /// <param name="roomAmenity">The data for creating a new room amenity.</param>
        /// <returns>Returns the created room amenity details.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<RoomAmenityResponse>>
            CreateRoomAmenityAsync(RoomAmenityForCreationRequest roomAmenity)
        {
            var request = _mapper.Map<CreateRoomAmenityCommand>(roomAmenity);
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
        /// <param name="roomAmenityForUpdateDto">The data for updating the room amenity.</param>
        /// <returns>Indicates successful update.</returns>
        [HttpPut("{roomAmenityId:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateRoomAmenity(Guid roomAmenityId,
        RoomAmenityForUpdateRequest roomAmenityForUpdateDto)
        {
            var request = _mapper.Map<UpdateRoomAmenityCommand>(roomAmenityForUpdateDto);
            request.Id = roomAmenityId;
            await _mediator.Send(request);

            var response = ApiResponse<string>.SuccessResponse(null, "Room Amenity updated successfully.");
            return Ok(response);
        }

        /// <summary>
        /// Deletes a room amenity with the specified ID.
        /// </summary>
        /// <param name="roomAmenityId">The unique identifier for the room amenity.</param>
        /// <returns>Indicates successful deletion.</returns>
        [HttpDelete("{roomAmenityId:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteHotel(Guid roomAmenityId)
        {
            var deleteRoomAmenityCommand = new DeleteRoomAmenityCommand { Id = roomAmenityId };
            await _mediator.Send(deleteRoomAmenityCommand);

            var response = ApiResponse<string>.SuccessResponse(null, "Room Amenity deleted successfully.");
            return Ok(response);
        }
    }
}