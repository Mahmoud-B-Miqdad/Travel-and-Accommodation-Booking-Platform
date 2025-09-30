using Stripe;
using System.Text;
using System.Text.Json;
using TravelEase.API.Common.Responses;

namespace TravelEase.API.Middlewares
{
    public class StripeWebhookVerificationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;

        public StripeWebhookVerificationMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path.StartsWithSegments("/api/webhooks/stripe") &&
                context.Request.Method.Equals("POST", StringComparison.OrdinalIgnoreCase))
            {
                string json;

                context.Request.EnableBuffering();

                using (var reader = new StreamReader(
                    context.Request.Body,
                    encoding: Encoding.UTF8,
                    detectEncodingFromByteOrderMarks: false,
                    bufferSize: 1024,
                    leaveOpen: true))
                {
                    json = await reader.ReadToEndAsync();
                    context.Request.Body.Position = 0;
                }

                var webhookSecret = _configuration["Stripe:WebhookSecret"];
                var stripeSignature = context.Request.Headers["Stripe-Signature"].ToString();

                try
                {
                    var stripeEvent = EventUtility.ConstructEvent(json, stripeSignature, webhookSecret);
                    context.Items["StripeEvent"] = stripeEvent;

                    await _next(context);
                }
                catch (StripeException ex)
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    context.Response.ContentType = "application/json";

                    var errorResponse = ApiResponse<string>.FailResponse("Invalid Stripe signature.");
                    var errorJson = JsonSerializer.Serialize(errorResponse);

                    await context.Response.WriteAsync(errorJson);
                    return;
                }
                catch (Exception ex)
                {
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    context.Response.ContentType = "application/json";

                    var errorResponse = ApiResponse<string>.FailResponse("An unexpected error occurred.");
                    var errorJson = JsonSerializer.Serialize(errorResponse);

                    await context.Response.WriteAsync(errorJson);
                    return;
                }
            }
            else
            {
                await _next(context);
            }
        }
    }
}