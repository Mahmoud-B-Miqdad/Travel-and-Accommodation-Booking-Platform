using Microsoft.Extensions.DependencyInjection;
using TravelEase.Application.CityManagement.Mapping;
using TravelEase.Application.HotelManagement.Mapping;
using TravelEase.Application.ReviewsManagement.Mapping;
using TravelEase.Application.RoomAmenityManagement.Mapping;
using TravelEase.Application.UserManagement.Mapping;

namespace TravelEase.Application
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            var assembly = typeof(ApplicationServiceExtensions).Assembly;

            services.AddMediatR(configuration =>
                configuration.RegisterServicesFromAssembly(assembly));
            services.AddAutoMapper(typeof(CityProfile));
            services.AddAutoMapper(typeof(HotelProfile));
            services.AddAutoMapper(typeof(RoomAmenityProfile));
            services.AddAutoMapper(typeof(ReviewsProfile));
            services.AddAutoMapper(typeof(UserProfile));

            return services;
        }
    }
}