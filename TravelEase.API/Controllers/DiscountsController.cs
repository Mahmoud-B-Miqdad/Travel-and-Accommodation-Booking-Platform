using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TravelEase.API.Common.Responses;
using TravelEase.Application.DiscountManagement.Commands;
using TravelEase.Application.DiscountManagement.DTOs.Requests;
using TravelEase.Application.DiscountManagement.DTOs.Responses;
using TravelEase.Application.DiscountManagement.Queries;
using TravelEase.Domain.Aggregates.Hotels;

namespace TravelEase.API.Controllers
{
    [Route("api/room-types/{roomTypeId:guid}/discounts")]
    [ApiController]
    public class DiscountsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        public DiscountsController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        /// <summary>
        /// Retrieves a paginated list of discounts for a specific roomType.
        /// </summary>
        /// <param name="roomTypeId">The ID of the hotel for which bookings are requested.</param>
        /// <param name="discountQueryRequest">DTO containing parameters for pagination and filtering.</param>
        /// <returns>
        /// Returns a paginated list of discounts for the specified roomType.
        /// </returns>
        /// <response code="200">Returns a paginated list of discounts.</response>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<List<DiscountResponse>>), StatusCodes.Status200OK)]
        [Authorize]
        public async Task<ActionResult<ApiResponse<List<DiscountResponse>>>>
            GetAllDiscountsByRoomTypeIdAsync(Guid roomTypeId,
            [FromQuery] DiscountQueryRequest discountQueryRequest)
        {
            var baseQuery = _mapper.Map<GetAllDiscountsByRoomTypeQuery>(discountQueryRequest);
            var request = baseQuery with
            {
                RoomTypeId = roomTypeId
            };

            var paginatedListOfDiscount = await _mediator.Send(baseQuery);
            Response.Headers.Append("X-Pagination",
                JsonSerializer.Serialize(paginatedListOfDiscount.PageData));

            var response = ApiResponse<List<DiscountResponse>>
                .SuccessResponse(paginatedListOfDiscount.Items);
            return Ok(response);
        }

        /// <summary>
        /// Gets a specific discount by its ID within a specific roomType.
        /// </summary>
        /// <param name="discountId">The unique identifier of the discount.</param>
        /// <param name="roomTypeId">RoomType ID.</param>
        /// <returns>The details of the requested discount.</returns>
        [HttpGet("{discountId:guid}", Name = "GetDiscountByIdAndRoomTypeId")]
        [ProducesResponseType(typeof(ApiResponse<DiscountResponse>), StatusCodes.Status200OK)]
        [Authorize]
        public async Task<ActionResult<ApiResponse<DiscountResponse>>>
            GetDiscountByIdAndRoomTypeIdAsync(Guid discountId, Guid roomTypeId)
        {
            var request = new GetDiscountByIdAndRoomTypeIdQuery
            {
                DiscountId = discountId,
                RoomTypeId = roomTypeId
            };
            var result = await _mediator.Send(request);

            var response = ApiResponse<DiscountResponse>.SuccessResponse(result);
            return Ok(response);
        }

        /// <summary>
        /// Creates a new discount based on the provided data.
        /// </summary>
        /// <param name="discountRequest">The data for creating a new discount.</param>
        /// <param name="roomTypeId">The unique identifier of the roomType.</param>
        /// <returns>Returns the created discount details.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<DiscountResponse>), StatusCodes.Status201Created)]
        [Authorize(Policy = "AdminOrOwner")]
        public async Task<ActionResult<ApiResponse<DiscountResponse>>>
            CreateRoomForHotelAsync(DiscountForCreationRequest discountRequest, Guid roomTypeId)
        {
            var baseCommand = _mapper.Map<CreateDiscountCommand>(discountRequest);
            var request = baseCommand with
            {
                RoomTypeId = roomTypeId
            };
            var discountToReturn = await _mediator.Send(baseCommand);

            var response = ApiResponse<DiscountResponse>.SuccessResponse(discountToReturn,
                "Discount created successfully");

            return CreatedAtAction("GetDiscountByIdAndRoomTypeId",
            new
            {
                roomTypeId,
                discountId = discountToReturn.Id
            }, response);
        }

        /// <summary>
        /// Deletes a specific discount by its unique identifier.
        /// </summary>
        /// <param name="discountId">The ID of the discount to delete.</param>
        /// <param name="roomTypeId">RoomType ID.</param>
        /// <returns>200 Ok Response if deletion is successful.</returns>
        [HttpDelete("{discountId:guid}")]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        [Authorize(Policy = "AdminOrOwner")]
        public async Task<ActionResult<ApiResponse<string>>> 
            DeleteBooking(Guid roomTypeId, Guid discountId)
        {
            var deleteBookingCommand = new DeleteDiscountCommand
            {
                RoomTypeId = roomTypeId,
                DiscountId = discountId,
            };

            await _mediator.Send(deleteBookingCommand);

            var response = ApiResponse<string>.SuccessResponse(null, "Discount deleted successfully.");
            return Ok(response);
        }
    }
}