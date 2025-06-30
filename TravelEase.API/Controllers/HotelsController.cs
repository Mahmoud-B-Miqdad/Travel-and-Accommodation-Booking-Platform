using AutoMapper;
using MediatR;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using TravelEase.Application.HotelManagement.Queries;
using TravelEase.API.Common.Responses;
using TravelEase.Application.HotelManagement.DTOs.Responses;
using TravelEase.Application.HotelManagement.Commands;
using TravelEase.Application.HotelManagement.DTOs.Requests;

namespace TravelEase.API.Controllers
{
    [Route("api/Hotels")]
    [ApiController]
    [ApiVersion("1.0")]
    public class HotelsController : ControllerBase
    {

        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public HotelsController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }
        /// <summary>
        /// Retrieves all hotels.
        /// </summary>
        /// <param name="getAllHotelsQuery">Optional parameters for filtering and pagination.</param>
        /// <returns>
        /// - 200 OK: If the hotels are successfully retrieved.
        /// - 401 Unauthorized: If the user is not authorized to access the resource.
        /// - 403 Forbidden: If the user is not allowed to access the resource.
        /// - 500 Internal Server Error: If an unexpected error occurs.
        /// </returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllHotelsAsync([FromQuery] GetAllHotelsQuery getAllHotelsQuery)
        {
            var paginatedListOfHotels = await _mediator.Send(getAllHotelsQuery);
            Response.Headers.Append("X-Pagination", 
                JsonSerializer.Serialize(paginatedListOfHotels.PageData));

            return Ok(ApiResponse<List<HotelWithoutRoomsResponse>>.SuccessResponse(paginatedListOfHotels.Items));
        }

        /// <summary>
        /// Retrieves information about a hotel based on its unique identifier (GUID).
        /// </summary>
        /// <param name="hotelId">The unique identifier of the hotel.</param>
        /// <returns>
        /// - 200 OK: If the hotel information is successfully retrieved.
        /// - 404 Not Found: If the hotel with the given ID does not exist.
        /// - 500 Internal Server Error: If an unexpected error occurs.
        /// </returns>
        [HttpGet("{hotelId:guid}", Name = "GetHotel")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetHotelAsync(Guid hotelId)
        {
            var request = new GetHotelByIdQuery { Id = hotelId };
            var hotel = await _mediator.Send(request);

            return Ok(ApiResponse<HotelWithoutRoomsResponse>.SuccessResponse(hotel!));
        }

        /// <summary>
        /// Creates a new hotel.
        /// </summary>
        /// <param name="hotel">The details of the hotel to be created.</param>
        /// <returns>
        /// - 201 Created: If the hotel is successfully created.
        /// - 400 Bad Request: If there are validation errors in the hotel data or if the request is malformed.
        /// - 401 Unauthorized: If the user is not authorized to create a hotel.
        /// - 500 Internal Server Error: If an unexpected error occurs.
        /// </returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<HotelWithoutRoomsResponse>> CreateHotelAsync(HotelForCreationRequest hotel)
        {
            var request = _mapper.Map<CreateHotelCommand>(hotel);
            var createdHotel = await _mediator.Send(request);

            var response = ApiResponse<HotelWithoutRoomsResponse>.SuccessResponse(createdHotel,
                "Hotel created successfully");

            return CreatedAtRoute("GetHotel",
            new
            {
                hotelId = createdHotel.Id
            },response);
        }

        /// <summary>
        /// Updates information about a hotel based on its unique identifier (GUID).
        /// </summary>
        /// <param name="hotelId">The unique identifier of the hotel.</param>
        /// <param name="hotelForUpdateDto">The updated information for the hotel.</param>
        /// <returns>
        /// - 200 Ok Response: If the hotel information is successfully updated.
        /// - 400 Bad Request: If there are validation errors in the updated
        /// hotel information or if a data constraint violation occurs.
        /// - 500 Internal Server Error: If an unexpected error occurs.
        /// </returns>
        [HttpPut("{hotelId:guid}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdateHotel(Guid hotelId,
        HotelForUpdateRequest hotelForUpdateDto)
        {
            var request = _mapper.Map<UpdateHotelCommand>(hotelForUpdateDto);
            request.Id = hotelId;
            await _mediator.Send(request);

            var response = ApiResponse<string>.SuccessResponse(null, "Hotel updated successfully.");
            return Ok(response);
        }

        /// <summary>
        /// Deletes a specific hotel by its unique identifier.
        /// </summary>
        /// <param name="hotelId">The ID of the hotel to delete.</param>
        /// <returns>200 Ok Response if deletion is successful.</returns>
        [HttpDelete("{hotelId:guid}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteHotel(Guid hotelId)
        {
            var deleteHotelCommand = new DeleteHotelCommand { Id = hotelId };
            await _mediator.Send(deleteHotelCommand);

            var response = ApiResponse<string>.SuccessResponse(null, "Hotel deleted successfully.");
            return Ok(response);
        }
    }
}