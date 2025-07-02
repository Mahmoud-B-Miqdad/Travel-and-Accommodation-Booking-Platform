using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TravelEase.API.Common.Responses;
using TravelEase.Application.BookingManagement.Commands;
using TravelEase.Application.BookingManagement.DTOs.Requests;
using TravelEase.Application.BookingManagement.DTOs.Responses;
using TravelEase.Application.CityManagement.DTOs.Responses;

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