using Microsoft.Extensions.DependencyInjection;
using TravelEase.Domain.Common.Interfaces;

namespace TravelEase.Infrastructure.Common.Security
{
    public static class SecurityServiceExtensions
    {
        public static IServiceCollection AddSecurityServices(this IServiceCollection services)
        {
            services.AddScoped<IPasswordHasher, Argon2PasswordHasher>();
            return services;
        }
    }
}