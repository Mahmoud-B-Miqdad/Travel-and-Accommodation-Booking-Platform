using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.Json;
using TravelEase.API.Common.Responses;
using TravelEase.Infrastructure.Persistence.Context;
using TravelEase.Infrastructure.Persistence.Services.SeedServices;

namespace TravelEase.API.Common.Extensions
{
    public static class WebApplicationExtensions
    {
        public static async Task ApplyMigrationsAndSeedAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<TravelEaseDbContext>();
            await dbContext.Database.MigrateAsync();

            var seeder = scope.ServiceProvider.GetRequiredService<SeedService>();
            await seeder.SeedIfNeededAsync();
        }

        public static WebApplication UseCustomUnauthorizedHandler(this WebApplication app)
        {
            app.Use(async (context, next) =>
            {
                await next();

                if ((context.Response.StatusCode == 401 || 
                context.Response.StatusCode == 403) && !context.Response.HasStarted)
                {
                    if (context.Response.StatusCode == 401)
                        context.Response.Headers.Remove("WWW-Authenticate");

                    context.Response.ContentType = "application/json";

                    var message = context.Response.StatusCode == 401
                        ? "Unauthorized access."
                        : "Forbidden. You do not have permission to access this resource.";

                    var apiResponse = ApiResponse<string>.FailResponse(message);
                    var json = JsonSerializer.Serialize(apiResponse);
                    context.Response.ContentLength = Encoding.UTF8.GetByteCount(json);
                    await context.Response.WriteAsync(json);
                }
            });

            return app;
        }
    }
}