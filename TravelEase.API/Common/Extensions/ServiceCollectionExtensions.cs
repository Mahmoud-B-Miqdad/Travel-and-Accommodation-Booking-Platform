using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using TravelEase.API.Common.Responses;
using TravelEase.Application;
using TravelEase.Infrastructure.Common.Extensions;
using TravelEase.Domain.Common.Models.SettingModels;
using SendGrid;
using Stripe;
using TravelEase.Infrastructure.Persistence.Services.SeedServices;
using Microsoft.AspNetCore.Mvc.Versioning;
using TravelEase.Application.CityManagement.Validators;

namespace TravelEase.API.Common.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices
        (this IServiceCollection services, IConfiguration config)
    {
        services.AddInfrastructure(config);
        services.AddApplication();
        services.AddScoped<SeedService>();

        services.Configure<CloudinarySettings>(config.GetSection("CLOUDINARY"));
        services.Configure<StripeSettings>(config.GetSection("Stripe"));

        services.AddControllers(options =>
        {
            options.ReturnHttpNotAcceptable = false;
        })
        .AddJsonOptions(opts =>
        {
            opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        })
        .AddNewtonsoftJson()
        .AddXmlDataContractSerializerFormatters();

        services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ReportApiVersions = true;
            options.ApiVersionReader = new HeaderApiVersionReader("x-api-version");
        });

        services.AddFluentValidationAutoValidation();
        services.AddValidatorsFromAssemblyContaining(typeof(GetAllCitiesQueryValidator));

        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.InvalidModelStateResponseFactory = context =>
            {
                var errors = context.ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                var response = ApiResponse<string>.FailResponse("Validation failed");
                response.Errors = errors;

                return new BadRequestObjectResult(response);
            };
        });

        services.AddEndpointsApiExplorer();

        services.AddSingleton<ISendGridClient>(sp =>
        {
            var apiKey = config["EmailSettings:ApiKey"];
            return new SendGridClient(apiKey);
        });

        var stripeSettings = config.GetSection("Stripe").Get<StripeSettings>();
        StripeConfiguration.ApiKey = stripeSettings.SecretKey;

        return services;
    }
}