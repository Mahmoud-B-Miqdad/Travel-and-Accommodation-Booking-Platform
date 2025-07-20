using FluentValidation;
using FluentValidation.AspNetCore;
using System.Reflection;
using TravelEase.API.Validators.CityValidators;
using TravelEase.Infrastructure.Common.Extensions;
using TravelEase.Application;
using TravelEase.API.Middlewares;
using Microsoft.AspNetCore.Mvc;
using TravelEase.API.Common.Responses;
using Microsoft.OpenApi.Models;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json;
using SendGrid;
using TravelEase.Infrastructure.Persistence.Services.SeedServices;
using TravelEase.Domain.Common.Models.ImageModels;

DotNetEnv.Env.Load();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddScoped<SeedService>();
builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CLOUDINARY"));

builder.Services.AddControllers(options =>
{
    options.ReturnHttpNotAcceptable = false;
}).AddNewtonsoftJson()
  .AddXmlDataContractSerializerFormatters();

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<GetAllCitiesQueryValidator>();

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState
            .Values
            .SelectMany(v => v.Errors)
            .Select(e => e.ErrorMessage)
            .ToList();

        var response = ApiResponse<string>.FailResponse("Validation failed");
        response.Errors = errors;

        return new BadRequestObjectResult(response);
    };
});


builder.Services.AddEndpointsApiExplorer();

builder.Services.AddAuthentication("Bearer").AddJwtBearer(
    options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Authentication:Issuer"],
            ValidAudience = builder.Configuration["Authentication:Audience"],
            IssuerSigningKey =
                new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["Authentication:SecretForKey"]))
        };
    });

builder.Services.AddSwaggerGen(setupAction =>
{
    var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);
    setupAction.IncludeXmlComments(xmlCommentsFullPath);
    setupAction.AddSecurityDefinition("TravelEaseApiAuth", new OpenApiSecurityScheme()
    {
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        Description = "Input a valid token to access this API"
    });
    setupAction.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme{
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "TravelEaseApiAuth"}
            },
            new List<string>()
        }
    });
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("MustBeAdmin", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireRole("Admin");
    });

    options.AddPolicy("MustBeOwner", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireRole("Owner");
    });

    options.AddPolicy("MustBeGuest", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireRole("Guest");
    });

    options.AddPolicy("AdminOrOwner", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireRole("Admin", "Owner");
    });
});

builder.Services.AddSingleton<ISendGridClient>(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    var apiKey = configuration["EmailSettings:ApiKey"];
    return new SendGridClient(apiKey);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<SeedService>();
    await seeder.SeedIfNeededAsync();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.Use(async (context, next) =>
{
    await next();

    if ((context.Response.StatusCode == 401 || context.Response.StatusCode == 403) && !context.Response.HasStarted)
    {
        if (context.Response.StatusCode == 401)
        {
            context.Response.Headers.Remove("WWW-Authenticate");
        }

        context.Response.ContentType = "application/json";

        var message = context.Response.StatusCode == 401
            ? "Unauthorized access."
            : "Forbidden. You do not have permission to access this resource.";

        var apiResponse = ApiResponse<string>.FailResponse(message);
        var responseJson = JsonSerializer.Serialize(apiResponse);

        context.Response.ContentLength = System.Text.Encoding.UTF8.GetByteCount(responseJson);
        await context.Response.WriteAsync(responseJson);
    }
});

app.UseAuthorization();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapControllers();

app.Run();