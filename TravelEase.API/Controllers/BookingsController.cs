using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TravelEase.API.Common.Responses;
using TravelEase.Application.BookingManagement.Commands;
using TravelEase.Application.BookingManagement.DTOs.Requests;
using TravelEase.Application.BookingManagement.DTOs.Responses;
using TravelEase.Application.BookingManagement.Queries;
using System.Text.Json;

namespace TravelEase.API.Controllers
{
    [Route("api/bookings")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        public BookingsController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        /// <summary>
        /// Retrieves a paginated list of bookings for a specific hotel.
        /// </summary>
        /// <param name="hotelId">The ID of the hotel for which bookings are requested.</param>
        /// <param name="bookingQueryRequest">DTO containing parameters for pagination and filtering.</param>
        /// <returns>
        /// Returns a paginated list of bookings for the specified hotel.
        /// </returns>
        /// <response code="200">Returns a paginated list of bookings.</response>
        [HttpGet("{hotelId:guid}")]
        [ProducesResponseType(typeof(ApiResponse<List<BookingResponse>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<List<BookingResponse>>>> 
            GetAllBookingsByHotelIdAsync(Guid hotelId,
            [FromQuery] BookingQueryRequest bookingQueryRequest)
        {
            var bookingQuery = _mapper.Map<GetAllBookingsByHotelIdQuery>(bookingQueryRequest);
            bookingQuery.HotelId = hotelId;

            var paginatedListOfBooking = await _mediator.Send(bookingQuery);
            Response.Headers.Append("X-Pagination",
                JsonSerializer.Serialize(paginatedListOfBooking.PageData));

            return Ok(ApiResponse<List<BookingResponse>>.SuccessResponse(paginatedListOfBooking.Items));
        }

        /// <summary>
        /// Retrieves a specific booking by its unique identifier.
        /// </summary>
        /// <param name="bookingId">The unique identifier of the booking.</param>
        /// <returns>The details of the requested booking.</returns>
        [HttpGet("{bookingId:guid}", Name = "GetBooking")]
        [ProducesResponseType(typeof(ApiResponse<BookingResponse>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<BookingResponse>>> GetBookingAsync(Guid bookingId)
        {
            var request = new GetBookingByIdQuery { Id = bookingId };
            var result = await _mediator.Send(request);
            var bookingResponse = _mapper.Map<BookingResponse>(result);

            return Ok(ApiResponse<BookingResponse>.SuccessResponse(bookingResponse));
        }

        /// <summary>
        /// Reserve a room.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/guests/bookings
        ///     {
        ///        "roomId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///        "checkInDate": "2025-07-01",
        ///        "checkOutDate": "2025-07-03"
        ///     }
        ///
        /// </remarks>
        /// <param name="bookingRequest">Booking details</param>
        [HttpPost("reserve-room")]
        [ProducesResponseType(typeof(ApiResponse<BookingResponse>), StatusCodes.Status201Created)]
        [Authorize]
        public async Task<ActionResult<ApiResponse<BookingResponse>>> ReserveRoomForAuthenticatedGuestAsync(ReserveRoomRequest bookingRequest)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var emailClaim = identity!.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

            var request = _mapper.Map<ReserveRoomCommand>(bookingRequest);
            request.GuestEmail = emailClaim!;
            var createdBooking = await _mediator.Send(request);

            var response = ApiResponse<BookingResponse>.SuccessResponse(createdBooking,
                "Booking has been successfully submitted!");

            return CreatedAtRoute("GetBooking",
            new
            {
                bookingId = createdBooking.Id
            }, response);
        }

        /// <summary>
        /// Deletes a specific booking by its unique identifier.
        /// </summary>
        /// <param name="bookingId">The ID of the booking to delete.</param>
        /// <returns>200 Ok Response if deletion is successful.</returns>
        [HttpDelete("{bookingId:guid}")]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        [Authorize]
        public async Task<ActionResult<ApiResponse<string>>> DeleteBooking(Guid bookingId)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var emailClaim = identity?.Claims.
                FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

            var deleteBookingCommand = new DeleteBookingCommand
            {
                Id = bookingId,
                GuestEmail = emailClaim!
            };

            await _mediator.Send(deleteBookingCommand);

            var response = ApiResponse<string>.SuccessResponse(null, "Booking deleted successfully.");
            return Ok(response);
        }
    }
}