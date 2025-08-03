using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TravelEase.API.Common.Extensions;
using TravelEase.API.Common.Responses;
using TravelEase.Application.ReviewsManagement.Commands;
using TravelEase.Application.ReviewsManagement.DTOs.Requests;
using TravelEase.Application.ReviewsManagement.DTOs.Responses;
using TravelEase.Application.ReviewsManagement.Queries;

namespace TravelEase.API.Controllers
{
    [Route("api/hotels/{hotelId:guid}/reviews")]
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
        /// <param name="reviewQueryRequest">DTO containing parameters for pagination and filtering.</param>
        /// <returns>
        /// Returns a paginated list of reviews for the specified hotel.
        /// </returns>
        /// <response code="200">Returns a paginated list of reviews.</response>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<List<ReviewResponse>>), StatusCodes.Status200OK)]
        [Authorize]
        public async Task<ActionResult<ApiResponse<List<ReviewResponse>>>> 
            GetAllReviewsByHotelIdAsync(Guid hotelId,
            [FromQuery] ReviewQueryRequest reviewQueryRequest)
        {
            var baseQuery = _mapper.Map<GetAllReviewsByHotelIdQuery>(reviewQueryRequest);
            var request = baseQuery with
            {
                HotelId = hotelId,
            };
            var paginatedListOfReviews = await _mediator.Send(baseQuery);
            Response.Headers.Append("X-Pagination",
                JsonSerializer.Serialize(paginatedListOfReviews.PageData));

            var response = ApiResponse<List<ReviewResponse>>.SuccessResponse(paginatedListOfReviews.Items);
            return Ok(response);
        }

        /// <summary>
        /// Gets a specific review by its ID within a specific hotel.
        /// </summary>
        /// <param name="reviewId">The unique identifier of the review.</param>
        /// <param name="hotelId">Hotel ID.</param>
        /// <returns>The details of the requested review.</returns>
        [HttpGet("{reviewId:guid}", Name = "GetReviewByIdAndHotelId")]
        [ProducesResponseType(typeof(ApiResponse<ReviewResponse>), StatusCodes.Status200OK)]
        [Authorize(Policy = "AdminOrOwner")]
        public async Task<ActionResult<ApiResponse<ReviewResponse>>> 
            GetReviewByIdAndHotelIdAsync(Guid reviewId, Guid hotelId)
        {
            var request = new GetReviewByIdAndHotelIdQuery
            {
                ReviewId = reviewId,
                HotelId = hotelId
            };
            var result = await _mediator.Send(request);

            var response = ApiResponse<ReviewResponse>.SuccessResponse(result);
            return Ok(response);
        }

        /// <summary>
        /// Creates a new review.
        /// </summary>
        /// <param name="reviewRequest">DTO containing review data.</param>
        /// <param name="hotelId">Hotel ID.</param>
        /// <returns>
        /// Returns the created review if successful.
        /// <returns>
        /// - 201 Created: If the review is successfully created.
        /// </returns>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<ReviewResponse>), StatusCodes.Status201Created)]
        [Authorize(Policy = "MustBeGuest")]
        public async Task<ActionResult<ApiResponse<ReviewResponse>>>
            CreateReviewAsync(ReviewForCreationRequest reviewRequest, Guid hotelId)
        {
            var email = User.GetEmailOrThrow();

            var baseCommand = _mapper.Map<CreateReviewCommand>(reviewRequest);
            var request = baseCommand with
            {
                HotelId = hotelId,
                GuestEmail = email!
            };
            var createdReview = await _mediator.Send(baseCommand);

            var response = ApiResponse<ReviewResponse>.SuccessResponse(createdReview,
                "Review submitted successfully!");

            return CreatedAtRoute("GetReviewByIdAndHotelId",
            new
            {
                hotelId,
                reviewId = createdReview.Id
            }, response);
        }
    }
}