using AutoMapper;
using MediatR;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using TravelEase.Application.HotelManagement.Queries;
using TravelEase.API.Common.Responses;
using TravelEase.Application.HotelManagement.DTOs.Responses;
using TravelEase.Application.HotelManagement.Commands;
using TravelEase.Application.HotelManagement.DTOs.Requests;
using TravelEase.Application.CityManagement.DTOs.Responses;

namespace TravelEase.API.Controllers
{
    [Route("api/hotels")]
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
        /// </returns>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<List<HotelWithoutRoomsResponse>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<List<HotelWithoutRoomsResponse>>>>
            GetAllHotelsAsync([FromQuery] GetAllHotelsQuery getAllHotelsQuery)
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
        /// </returns>
        [HttpGet("{hotelId:guid}", Name = "GetHotel")]
        [ProducesResponseType(typeof(ApiResponse<HotelWithoutRoomsResponse>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<HotelWithoutRoomsResponse>>> GetHotelAsync(Guid hotelId)
        {
            var request = new GetHotelByIdQuery { Id = hotelId };
            var hotel = await _mediator.Send(request);

            return Ok(ApiResponse<HotelWithoutRoomsResponse>.SuccessResponse(hotel!));
        }

        /// <summary>
        /// Creates a new hotel.
        /// </summary>
        /// <param name="hotelRequest">The details of the hotel to be created.</param>
        /// <returns>
        /// - 201 Created: If the hotel is successfully created.
        /// </returns>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<HotelWithoutRoomsResponse>), StatusCodes.Status201Created)]
        public async Task<ActionResult<ApiResponse<HotelWithoutRoomsResponse>>>
            CreateHotelAsync(HotelForCreationRequest hotelRequest)
        {
            var request = _mapper.Map<CreateHotelCommand>(hotelRequest);
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
        /// <param name="hotelForUpdateRequest">The updated information for the hotel.</param>
        /// <returns>
        /// - 200 Ok Response: If the hotel information is successfully updated.
        /// </returns>
        [HttpPut("{hotelId:guid}")]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<string>>> UpdateHotel(Guid hotelId,
        HotelForUpdateRequest hotelForUpdateRequest)
        {
            var request = _mapper.Map<UpdateHotelCommand>(hotelForUpdateRequest);
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
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<string>>> DeleteHotel(Guid hotelId)
        {
            var deleteHotelCommand = new DeleteHotelCommand { Id = hotelId };
            await _mediator.Send(deleteHotelCommand);

            var response = ApiResponse<string>.SuccessResponse(null, "Hotel deleted successfully.");
            return Ok(response);
        }
    }
}