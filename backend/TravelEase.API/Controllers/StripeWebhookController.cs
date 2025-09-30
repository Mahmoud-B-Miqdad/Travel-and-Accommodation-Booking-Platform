using MediatR;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using TravelEase.Application.PaymentManagement.Commands;
using TravelEase.Domain.Common.Interfaces;

namespace TravelEase.API.Controllers
{
    [Route("api/webhooks/stripe")]
    [ApiController]
    public class StripeWebhookController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;

        public StripeWebhookController
            (IMediator mediator, IConfiguration configuration, IUnitOfWork unitOfWork)
        {
            _mediator = mediator;
            _configuration = configuration;
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        public async Task<IActionResult> Handle()
        {
            if (!HttpContext.Items.TryGetValue("StripeEvent", out var stripeEventObj) || stripeEventObj is not Event stripeEvent)
                return BadRequest("Invalid webhook event.");

            await _mediator.Send(new ProcessStripeWebhookCommand { StripeEvent = stripeEvent});

            return Ok();
        }
    }
}