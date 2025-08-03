using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelEase.API.Common.Responses;
using TravelEase.Application.BookingManagement.Commands;
using TravelEase.Application.BookingManagement.DTOs.Requests;
using TravelEase.Application.BookingManagement.DTOs.Responses;
using TravelEase.Application.BookingManagement.Queries;
using System.Text.Json;
using TravelEase.API.Common.Extensions;
using TravelEase.Domain.Common.Interfaces;
using TravelEase.Domain.Common.Models.CommonModels;

namespace TravelEase.API.Controllers
{
    [Route("api/hotels/{hotelId:guid}/bookings")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IInvoiceHtmlGenerator _invoiceHtmlGenerator;
        private readonly IPdfService _pdfService;
        private readonly IMapper _mapper;
        public BookingsController(IMediator mediator, IInvoiceHtmlGenerator invoiceHtmlGenerator,
           IPdfService pdfService, IMapper mapper)
        {
            _mediator = mediator;
            _invoiceHtmlGenerator = invoiceHtmlGenerator;
            _pdfService = pdfService;
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
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<List<BookingResponse>>), StatusCodes.Status200OK)]
        [Authorize(Policy = "AdminOrOwner")]
        public async Task<ActionResult<ApiResponse<List<BookingResponse>>>> 
            GetAllBookingsByHotelIdAsync(Guid hotelId,
            [FromQuery] BookingQueryRequest bookingQueryRequest)
        {
            var baseQuery = _mapper.Map<GetAllBookingsByHotelIdQuery>(bookingQueryRequest);
            var request = baseQuery with
            {
                HotelId = hotelId,
            };

            var paginatedListOfBooking = await _mediator.Send(baseQuery);
            Response.Headers.Append("X-Pagination",
                JsonSerializer.Serialize(paginatedListOfBooking.PageData));

            var response = ApiResponse<List<BookingResponse>>.SuccessResponse(paginatedListOfBooking.Items);
            return Ok(response);
        }

        /// <summary>
        /// Gets a specific booking by its ID within a specific hotel.
        /// </summary>
        /// <param name="bookingId">The unique identifier of the booking.</param>
        /// <param name="hotelId">Hotel ID.</param>
        /// <returns>The details of the requested booking.</returns>
        [HttpGet("{bookingId:guid}", Name = "GetBookingByIdAndHotelId")]
        [ProducesResponseType(typeof(ApiResponse<BookingResponse>), StatusCodes.Status200OK)]
        [Authorize]
        public async Task<ActionResult<ApiResponse<BookingResponse>>> 
            GetBookingByIdAndHotelIdAsync(Guid bookingId, Guid hotelId)
        {
            var request = new GetBookingByIdAndHotelIdQuery
            {
                BookingId = bookingId,
                HotelId = hotelId
            };
            var result = await _mediator.Send(request);

            var response = ApiResponse<BookingResponse>.SuccessResponse(result);
            return Ok(response);
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
        /// <param name="hotelId">Hotel ID.</param>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<BookingResponse>), StatusCodes.Status201Created)]
        [Authorize(Policy = "MustBeGuest")]
        public async Task<ActionResult<ApiResponse<BookingResponse>>>
            ReserveRoomAsync(Guid hotelId, ReserveRoomRequest bookingRequest)
        {
            var email = User.GetEmailOrThrow();

            var baseCommand  = _mapper.Map<ReserveRoomCommand>(bookingRequest);
            var request = baseCommand with
            {
                HotelId = hotelId,
                GuestEmail = email!
            };
            var createdBooking = await _mediator.Send(baseCommand );

            var response = ApiResponse<BookingResponse>.SuccessResponse(createdBooking,
                "Booking has been successfully submitted!");

            return CreatedAtRoute("GetBookingByIdAndHotelId",
            new
            {
                hotelId,
                bookingId = createdBooking.Id
            }, response);
        }

        /// <summary>
        /// Deletes a specific booking by its unique identifier.
        /// </summary>
        /// <param name="bookingId">The ID of the booking to delete.</param>
        /// <param name="hotelId">Hotel ID.</param>
        /// <returns>200 Ok Response if deletion is successful.</returns>
        [HttpDelete("{bookingId:guid}")]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        [Authorize]
        public async Task<ActionResult<ApiResponse<string>>> DeleteBooking(Guid hotelId, Guid bookingId)
        {
            var email = User.GetEmailOrThrow();

            var deleteBookingCommand = new DeleteBookingCommand
            {
                HotelId = hotelId,
                BookingId = bookingId,
                GuestEmail = email!
            };

            await _mediator.Send(deleteBookingCommand);

            var response = ApiResponse<string>.SuccessResponse(null, "Booking deleted successfully.");
            return Ok(response);
        }

        /// <summary>
        /// Retrieves the invoice for a specific hotel booking.
        ///
        /// Supports generating a PDF version and optionally sending the invoice via email.
        /// 
        /// </summary>
        /// <param name="hotelId">The unique identifier of the hotel.</param>
        /// <param name="bookingId">The unique identifier of the booking.</param>
        /// <param name="includePdf">If true, generates and returns the invoice as a PDF file.</param>
        /// <param name="sendEmail">If true, sends the invoice to the user's email address.</param>
        /// <returns>
        /// Returns the invoice data as JSON by default, or as a PDF file if <c>includePdf</c> is true and <c>sendEmail</c> is false.
        /// </returns>
        /// <response code="200">Returns the invoice as JSON or PDF based on query parameters.</response>

        [HttpGet("{bookingId:guid}/invoice")]
        [ProducesResponseType(typeof(ApiResponse<InvoiceResponse>), StatusCodes.Status200OK)]
        [Authorize]
        public async Task<ActionResult<ApiResponse<InvoiceResponse>>> GetInvoice
            (Guid hotelId, Guid bookingId, [FromQuery] bool includePdf = false,
            [FromQuery] bool sendEmail = false)
        {

            var email = User.GetEmailOrThrow();
            var name = User.GetFullNameOrEmpty();

            var query = new GetInvoiceByBookingIdQuery
            {
                HotelId = hotelId,
                BookingId = bookingId,
                GuestEmail = email,
                GuestName = name,
                GeneratePdf = includePdf,
                SendByEmail = sendEmail
            };

            var invoice = await _mediator.Send(query);

            if (includePdf && !sendEmail)
            {
                var pdfInvoice = _mapper.Map<Invoice>(invoice);
                var html = _invoiceHtmlGenerator.GenerateHtml(pdfInvoice, name);
                var pdfBytes = _pdfService.CreatePdfFromHtml(html);
                return File(pdfBytes, "application/pdf", "invoice.pdf");
            }

            var response = ApiResponse<InvoiceResponse>.SuccessResponse(invoice);
            return Ok(response);
        }
    }
}