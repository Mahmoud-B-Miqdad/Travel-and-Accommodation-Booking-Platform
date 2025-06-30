using AutoMapper;
using MediatR;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using TravelEase.API.Common.Responses;
using TravelEase.Application.RoomAmenityManagement.Query;
using TravelEase.Application.RoomAmenityManagement.DTOs.Responses;

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
    }
}