using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TravelEase.API.Common.Responses;
using TravelEase.Application.DiscountManagement.DTOs.Requests;
using TravelEase.Application.DiscountManagement.DTOs.Responses;
using TravelEase.Application.DiscountManagement.Queries;

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
            var discountQuery = _mapper.Map<GetAllDiscountsByRoomTypeQuery>(discountQueryRequest);
            discountQuery.RoomTypeId = roomTypeId;

            var paginatedListOfDiscount = await _mediator.Send(discountQuery);
            Response.Headers.Append("X-Pagination",
                JsonSerializer.Serialize(paginatedListOfDiscount.PageData));

            var response = ApiResponse<List<DiscountResponse>>
                .SuccessResponse(paginatedListOfDiscount.Items);
            return Ok(response);
        }
    }
}