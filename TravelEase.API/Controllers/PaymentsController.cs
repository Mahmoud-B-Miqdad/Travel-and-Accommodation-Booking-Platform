using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelEase.API.Common.Extensions;
using TravelEase.API.Common.Responses;
using TravelEase.Application.BookingManagement.Commands;
using TravelEase.Application.PaymentManagement.Commands;
using TravelEase.Application.PaymentManagement.DTOs.Requests;
using TravelEase.Domain.Aggregates.Hotels;
using TravelEase.Domain.Enums;

namespace TravelEase.API.Controllers
{
    [Route("api/payments")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public PaymentsController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        /// <summary>
        /// Creates a Stripe Payment Intent for the specified booking and payment details.
        /// </summary>
        /// <param name="createPaymentIntentRequest">The payment intent creation request containing booking ID, amount, and payment method.</param>
        /// <returns>
        /// Returns a successful API response containing the Stripe client secret string,
        /// which the client will use to complete the payment process.
        /// </returns>
        /// <remarks>
        /// The endpoint requires the user to be authorized.
        /// The user's email is extracted from the authentication claims and included in the command.
        /// </remarks>
        [HttpPost("create-payment-intent")]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        [Authorize]
        public async Task<ActionResult<ApiResponse<string>>> CreatePaymentIntent
            ([FromBody] CreatePaymentIntentRequest createPaymentIntentRequest)
        {
            var email = User.GetEmailOrThrow();

            var baseCommand = _mapper.Map<CreatePaymentIntentCommand>(createPaymentIntentRequest);
            var request = baseCommand with
            {
                GuestEmail = email!
            };
            var clientSecret = await _mediator.Send(baseCommand);

            var response = ApiResponse<string>.SuccessResponse(clientSecret);
            return Ok(response);
        }
    }
}