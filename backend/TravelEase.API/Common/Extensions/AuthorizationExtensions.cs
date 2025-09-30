namespace TravelEase.API.Common.Extensions
{
    public static class AuthorizationExtensions
    {
        public static IServiceCollection AddAuthorizationPolicies(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("MustBeAdmin", policy =>
                    policy.RequireAuthenticatedUser().RequireRole("Admin"));

                options.AddPolicy("MustBeOwner", policy =>
                    policy.RequireAuthenticatedUser().RequireRole("Owner"));

                options.AddPolicy("MustBeGuest", policy =>
                    policy.RequireAuthenticatedUser().RequireRole("Guest"));

                options.AddPolicy("AdminOrOwner", policy =>
                    policy.RequireAuthenticatedUser().RequireRole("Admin", "Owner"));
            });

            return services;
        }
    }
}