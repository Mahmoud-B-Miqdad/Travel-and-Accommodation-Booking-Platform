using System.Net;
using TravelEase.API.Common.Responses;
using TravelEase.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

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

                case InvalidOperationException invalidOpEx:
                    statusCode = HttpStatusCode.BadRequest;
                    apiResponse = ApiResponse<string>.FailResponse(invalidOpEx.Message);
                    break;

                case DbUpdateConcurrencyException concurrencyEx:
                    statusCode = HttpStatusCode.Conflict; 
                    apiResponse = ApiResponse<string>.FailResponse("The record you attempted to edit was modified or deleted by another process.");
                    break;

                case UnauthorizedAccessException unauthorizedEx:
                    statusCode = HttpStatusCode.Unauthorized;
                    apiResponse = ApiResponse<string>.FailResponse("Unauthorized access.");
                    break;

                case ForbiddenAccessException forbiddenEx: 
                    statusCode = HttpStatusCode.Forbidden;
                    apiResponse = ApiResponse<string>.FailResponse("Forbidden.");
                    break;


                default:
                    statusCode = HttpStatusCode.InternalServerError;
                    apiResponse = ApiResponse<string>.FailResponse("An unexpected error occurred.");
                    break;
            }

            context.Response.StatusCode = (int)statusCode;
            await context.Response.WriteAsJsonAsync(apiResponse);
        }
    }
}