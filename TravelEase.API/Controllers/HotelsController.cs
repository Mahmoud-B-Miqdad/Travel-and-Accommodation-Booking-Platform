using AutoMapper;
using MediatR;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using TravelEase.Application.HotelManagement.Queries;
using TravelEase.API.Common.Responses;
using TravelEase.Application.HotelManagement.DTOs.Responses;
using TravelEase.Application.HotelManagement.Commands;
using TravelEase.Application.HotelManagement.DTOs.Requests;
using TravelEase.Application.RoomManagement.DTOs.Responses;
using Microsoft.AspNetCore.Authorization;
using TravelEase.Application.ImageManagement.Queries;
using TravelEase.Domain.Common.Models.PaginationModels;
using TravelEase.Application.ImageManagement.Commands;

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
        [Authorize(Policy = "MustBeAdmin")]
        public async Task<ActionResult<ApiResponse<List<HotelWithoutRoomsResponse>>>>
            GetAllHotelsAsync([FromQuery] GetAllHotelsQuery getAllHotelsQuery)
        {
            var paginatedListOfHotels = await _mediator.Send(getAllHotelsQuery);
            Response.Headers.Append("X-Pagination", 
                JsonSerializer.Serialize(paginatedListOfHotels.PageData));

            var response = ApiResponse<List<HotelWithoutRoomsResponse>>.SuccessResponse(paginatedListOfHotels.Items);
            return Ok(response);
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
        [Authorize(Policy = "MustBeAdmin")]
        public async Task<ActionResult<ApiResponse<HotelWithoutRoomsResponse>>> GetHotelAsync(Guid hotelId)
        {
            var request = new GetHotelByIdQuery { Id = hotelId };
            var hotel = await _mediator.Send(request);

            var response = ApiResponse<HotelWithoutRoomsResponse>.SuccessResponse(hotel!);
            return Ok(response);
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
        [Authorize(Policy = "AdminOrOwner")]
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
        [Authorize(Policy = "AdminOrOwner")]
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
        [Authorize(Policy = "AdminOrOwner")]
        public async Task<ActionResult<ApiResponse<string>>> DeleteHotel(Guid hotelId)
        {
            var deleteHotelCommand = new DeleteHotelCommand { Id = hotelId };
            await _mediator.Send(deleteHotelCommand);

            var response = ApiResponse<string>.SuccessResponse(null, "Hotel deleted successfully.");
            return Ok(response);
        }

        /// <summary>
        /// Retrieves all photos associated with a hotel based on its unique identifier (GUID).
        /// </summary>
        /// <param name="hotelId">The unique identifier of the hotel.</param>
        /// <returns>
        /// - 200 OK: If the photos are successfully retrieved.
        /// - 404 Not Found: If the hotel with the given ID does not exist.
        /// - 500 Internal Server Error: If an unexpected error occurs.
        /// </returns>
        [HttpGet("{hotelId:guid}/photos")]
        [ProducesResponseType(typeof(ApiResponse<PaginatedList<string>>), StatusCodes.Status200OK)]
        [Authorize]
        public async Task<ActionResult<ApiResponse<PaginatedList<string>>>> GetAllHotelPhotos
            (Guid hotelId, int pageNumber = 1, int pageSize = 10)
        {
            var result = await _mediator.Send(new GetAllImagesQuery
            {
                HotelId = hotelId,
                PageNumber = pageNumber,
                PageSize = pageSize
            });

            var response = ApiResponse<PaginatedList<string>>.SuccessResponse(result);
            return Ok(response);
        }

        /// <summary>
        /// Uploads an image to the gallery of a specific hotel.
        /// </summary>
        /// <param name="hotelId">The unique identifier of the hotel.</param>
        /// <param name="file">The image file to upload.</param>
        /// <returns>Returns a success message if the upload is completed successfully.</returns>
        /// <response code="200">Image uploaded successfully.</response>
        /// <response code="401">Unauthorized – user is not authenticated.</response>
        /// <response code="403">Forbidden – user does not have the required admin permissions.</response>
        /// <remarks>
        /// This endpoint is restricted to administrators only (Requires 'MustBeAdmin' policy).
        /// </remarks>

        [HttpPost("{hotelId:guid}/gallery")]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        [Authorize(Policy = "MustBeAdmin")]
        public async Task<IActionResult> UploadImageForHotelAsync(Guid hotelId, IFormFile file)
        {
            var uploadHotelImageCommand = new UploadHotelImageCommand
            {
                HotelId = hotelId,
                File = file
            };

            await _mediator.Send(uploadHotelImageCommand);

            var response = ApiResponse<string>.SuccessResponse(null, "Image uploaded successfully.");
            return Ok(response);
        }
    }
}