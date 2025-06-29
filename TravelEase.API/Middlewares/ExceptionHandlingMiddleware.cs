using System.Net;
using TravelEase.API.Common.Responses;
using TravelEase.Domain.Exceptions;
using System.Text.Json;

namespace TravelEase.API.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception occurred.");
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            HttpStatusCode statusCode;
            object apiResponse;

            switch (exception)
            {
                case NotFoundException notFoundEx:
                    statusCode = HttpStatusCode.NotFound;
                    apiResponse = ApiResponse<string>.FailResponse(notFoundEx.Message);
                    break;

                case ConflictException conflictEx:
                    statusCode = HttpStatusCode.Conflict;
                    apiResponse = ApiResponse<string>.FailResponse(conflictEx.Message);
                    break;

                default:
                    statusCode = HttpStatusCode.InternalServerError;
                    apiResponse = ApiResponse<string>.FailResponse("An unexpected error occurred.");
                    break;
            }

            context.Response.StatusCode = (int)statusCode;
            var json = JsonSerializer.Serialize(apiResponse);
            await context.Response.WriteAsync(json);
        }
    }
}