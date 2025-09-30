using Serilog;
using TravelEase.API.Middlewares;
using TravelEase.API.Common.Extensions;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
    DotNetEnv.Env.Load();

builder.Host.UseSerilog((context, services, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration)
                 .ReadFrom.Services(services)
                 .Enrich.FromLogContext());

builder.Configuration.AddEnvironmentVariables();

builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddAuthenticationServices(builder.Configuration);
builder.Services.AddAuthorizationPolicies();
builder.Services.AddSwaggerDocumentation();

var app = builder.Build();

await app.ApplyMigrationsAndSeedAsync();

app.UseSwaggerIfNeeded();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseCustomUnauthorizedHandler();
app.UseAuthorization();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<StripeWebhookVerificationMiddleware>();

app.MapControllers();
app.Run();