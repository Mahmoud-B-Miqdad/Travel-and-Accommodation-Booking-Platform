using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TravelEase.API.Common.Responses;
using TravelEase.Application.ReviewsManagement.DTOs.Requests;
using TravelEase.Application.ReviewsManagement.DTOs.Responses;
using TravelEase.Application.ReviewsManagement.Queries;

namespace TravelEase.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class ReviewsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public ReviewsController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        /// <summary>
        /// Retrieves a paginated list of reviews for a specific hotel.
        /// </summary>
        /// <param name="hotelId">The ID of the hotel for which reviews are requested.</param>
        /// <param name="reviewQueryDto">DTO containing parameters for pagination and filtering.</param>
        /// <returns>
        /// Returns a paginated list of reviews for the specified hotel.
        /// </returns>
        /// <response code="200">Returns a paginated list of reviews.</response>
        [HttpGet("hotels/{hotelId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllReviewsByHotelIdAsync(Guid hotelId,
            [FromQuery] ReviewQueryRequest reviewQueryDto)
        {
            var reviewQuery = _mapper.Map<GetAllReviewsByHotelIdQuery>(reviewQueryDto);
            reviewQuery.HotelId = hotelId;

            var paginatedListOfReviews = await _mediator.Send(reviewQuery);
            Response.Headers.Append("X-Pagination",
                JsonSerializer.Serialize(paginatedListOfReviews.PageData));

            return Ok(ApiResponse<List<ReviewResponse>>.SuccessResponse(paginatedListOfReviews.Items));
        }
    }
}