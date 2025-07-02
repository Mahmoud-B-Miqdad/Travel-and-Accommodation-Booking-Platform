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
        /// Retrieves a specific booking by its unique identifier.
        /// </summary>
        /// <param name="bookingId">The unique identifier of the booking.</param>
        /// <returns>The details of the requested booking.</returns>
        [HttpGet("{bookingId:guid}", Name = "GetBooking")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetBookingAsync(Guid bookingId)
        {
            var request = new GetBookingByIdQuery { Id = bookingId };
            var result = await _mediator.Send(request);
            var bookingDto = _mapper.Map<BookingResponse>(result);

            return Ok(ApiResponse<BookingResponse>.SuccessResponse(bookingDto));
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
        /// <param name="booking">Booking details</param>
        [HttpPost("bookings")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [Authorize]
        public async Task<IActionResult> ReserveRoomForAuthenticatedGuestAsync(ReserveRoomRequest booking)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var emailClaim = identity!.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

            var request = _mapper.Map<ReserveRoomCommand>(booking);
            request.GuestEmail = emailClaim!;
            var createdBooking = await _mediator.Send(request);

            var response = ApiResponse<BookingResponse>.SuccessResponse(createdBooking,
                "Booking has been successfully submitted!");

            return CreatedAtRoute("GetBooking",
            new
            {
                cityId = createdBooking.Id
            }, response);
        }
    }
}