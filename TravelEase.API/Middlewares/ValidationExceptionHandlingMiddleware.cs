using TravelEase.API.Common.Responses;
using FluentValidation;

namespace TravelEase.API.Middlewares
{
    public class ValidationExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ValidationExceptionHandlingMiddleware> _logger;

        public ValidationExceptionHandlingMiddleware(RequestDelegate next, ILogger<ValidationExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning("Validation error: {Errors}", ex.Errors);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = StatusCodes.Status400BadRequest;

                var response = ApiResponse<string>.FailResponse("Validation failed", ex.Errors.Select(e => e.ErrorMessage).ToList());
                await context.Response.WriteAsJsonAsync(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception");
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                var response = ApiResponse<string>.FailResponse("An unexpected error occurred.");
                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }
}