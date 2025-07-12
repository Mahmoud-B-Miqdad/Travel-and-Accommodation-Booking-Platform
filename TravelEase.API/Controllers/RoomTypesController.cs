using AutoMapper;
using MediatR;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelEase.API.Common.Responses;
using TravelEase.Application.RoomTypeManagement.DTOs.Requests;
using TravelEase.Application.RoomTypeManagement.Queries;
using TravelEase.Application.RoomTypeManagement.DTOs.Responses;

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
        [AllowAnonymous]
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
    }
}