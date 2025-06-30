using AutoMapper;
using MediatR;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using TravelEase.Application.HotelManagement.Queries;
using TravelEase.API.Common.Responses;
using TravelEase.Application.CityManagement.DTOs.Responses;
using TravelEase.Application.HotelManagement.DTOs.Responses;
using Microsoft.AspNetCore.Authorization;

namespace TravelEase.API.Controllers
{
    [Route("api/Hotels")]
    [ApiController]
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
    }
}