using AutoMapper;
using MediatR;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelEase.API.Common.Responses;
using TravelEase.Application.RoomTypeManagement.DTOs.Requests;
using TravelEase.Application.RoomTypeManagement.Queries;
using TravelEase.Application.RoomTypeManagement.DTOs.Responses;
using TravelEase.Application.RoomTypeManagement.Commands;
using TravelEase.API.Common.Extensions;
using TravelEase.Application.BookingManagement.Commands;

namespace TravelEase.API.Controllers
{
    [Route("api/hotels/{hotelId:guid}/room-types")]
    [ApiController]
    [ApiVersion("1.0")]
    public class RoomTypesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public RoomTypesController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        /// <summary>
        /// Retrieves a paginated list of roomTypes for a specific hotel.
        /// </summary>
        /// <param name="hotelId">The ID of the hotel for which roomTypes are requested.</param>
        /// <param name="roomTypesQueryRequest">DTO containing parameters for pagination and filtering.</param>
        /// <returns>
        /// Returns a paginated list of roomTypes for the specified hotel.
        /// </returns>
        /// <response code="200">Returns a paginated list of roomTypes.</response>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<List<RoomTypeResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<List<RoomTypeWithoutAmenitiesResponse>>)
            , StatusCodes.Status200OK)]
        [Authorize]
        public async Task<ActionResult<ApiResponse<List<RoomTypeResponse>>>>
            GetAllRoomTypesByHotelIdAsync(Guid hotelId,
            [FromQuery] GetRoomTypesByHotelIdRequest roomTypesQueryRequest)
        {
            var roomTypeQuery = _mapper.Map<GetAllRoomTypesByHotelIdQuery>(roomTypesQueryRequest);
            roomTypeQuery.HotelId = hotelId;

            var paginatedListOfRoomTypes = await _mediator.Send(roomTypeQuery);
            Response.Headers.Append("X-Pagination",
                JsonSerializer.Serialize(paginatedListOfRoomTypes.PageData));

            if (!roomTypesQueryRequest.IncludeAmenities)
            {
                var result = _mapper.Map<List<RoomTypeWithoutAmenitiesResponse>>
                    (paginatedListOfRoomTypes.Items);
                return Ok(ApiResponse<List<RoomTypeWithoutAmenitiesResponse>>.SuccessResponse(result));
            }

            var response = ApiResponse<List<RoomTypeResponse>>
                .SuccessResponse(paginatedListOfRoomTypes.Items);
            return Ok(response);
        }

        /// <summary>
        /// Gets a specific roomType by its ID within a specific hotel.
        /// </summary>
        /// <param name="roomTypeId">The unique identifier of the roomType.</param>
        /// <param name="hotelId">Hotel ID.</param>
        /// <returns>The details of the requested roomType.</returns>
        [HttpGet("{roomTypeId:guid}", Name = "GetRoomTypeByIdAndHotelId")]
        [ProducesResponseType(typeof(ApiResponse<RoomTypeResponse>), StatusCodes.Status200OK)]
        [Authorize]
        public async Task<ActionResult<ApiResponse<RoomTypeResponse>>>
            GetRoomTypeByIdAndHotelIdAsync(Guid roomTypeId, Guid hotelId)
        {
            var request = new GetRoomTypeByIdAndHotelIdQuery
            {
                RoomTypeId = roomTypeId,
                HotelId = hotelId
            };
            var result = await _mediator.Send(request);

            var response = ApiResponse<RoomTypeResponse>.SuccessResponse(result);
            return Ok(response);
        }

        /// <summary>
        /// Creates a new roomType.
        /// </summary>
        /// <param name="roomTypeRequest">DTO containing roomType data.</param>
        /// <param name="hotelId">Hotel ID.</param>
        /// <returns>
        /// Returns the created roomType if successful.
        /// <returns>
        /// - 201 Created: If the rooType is successfully created.
        /// </returns>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<RoomTypeResponse>), StatusCodes.Status201Created)]
        [Authorize(Policy = "AdminOrOwner")]
        public async Task<ActionResult<ApiResponse<RoomTypeResponse>>>
            CreateRoomTypeAsync(RoomTypeForCreationRequest roomTypeRequest, Guid hotelId)
        {
            var request = _mapper.Map<CreateRoomTypeCommand>(roomTypeRequest);
            request.HotelId = hotelId;

            var createdRoomType = await _mediator.Send(request);

            var response = ApiResponse<RoomTypeResponse>.SuccessResponse(createdRoomType,
                "RoomType submitted successfully!");

            return CreatedAtRoute("GetRoomTypeByIdAndHotelId",
            new
            {
                hotelId,
                roomTypeId = createdRoomType.Id
            }, response);
        }

        /// <summary>
        /// Deletes a specific roomType by its unique identifier.
        /// </summary>
        /// <param name="roomTypeId">The ID of the roomType to delete.</param>
        /// <param name="hotelId">Hotel ID.</param>
        /// <returns>200 Ok Response if deletion is successful.</returns>
        [HttpDelete("{roomTypeId:guid}")]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        [Authorize(Policy = "AdminOrOwner")]
        public async Task<ActionResult<ApiResponse<string>>> DeleteRoomType(Guid hotelId, Guid roomTypeId)
        {
            var deleteBookingCommand = new DeleteRoomTypeCommand
            {
                HotelId = hotelId,
                RoomTypeId = roomTypeId,
            };

            await _mediator.Send(deleteBookingCommand);

            var response = ApiResponse<string>.SuccessResponse(null, "RoomType deleted successfully.");
            return Ok(response);
        }
    }
}