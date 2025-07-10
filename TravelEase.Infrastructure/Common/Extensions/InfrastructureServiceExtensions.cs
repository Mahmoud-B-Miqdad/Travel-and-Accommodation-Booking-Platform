using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TravelEase.Domain.Aggregates.Bookings;
using TravelEase.Domain.Aggregates.Cities;
using TravelEase.Domain.Aggregates.Discounts;
using TravelEase.Domain.Aggregates.Hotels;
using TravelEase.Domain.Aggregates.Reviews;
using TravelEase.Domain.Aggregates.RoomAmenities;
using TravelEase.Domain.Aggregates.Rooms;
using TravelEase.Domain.Aggregates.RoomTypes;
using TravelEase.Domain.Aggregates.Users;
using TravelEase.Domain.Common.Interfaces;
using TravelEase.Infrastructure.Persistence.Context;
using TravelEase.Infrastructure.Persistence.EntityPersistence.BookingPersistence;
using TravelEase.Infrastructure.Persistence.EntityPersistence.CityPersistence;
using TravelEase.Infrastructure.Persistence.EntityPersistence.DiscountPersistence;
using TravelEase.Infrastructure.Persistence.EntityPersistence.HotelPersistence;
using TravelEase.Infrastructure.Persistence.EntityPersistence.ReviewPersistence;
using TravelEase.Infrastructure.Persistence.EntityPersistence.RoomAmenityPersistence;
using TravelEase.Infrastructure.Persistence.EntityPersistence.RoomPersistence;
using TravelEase.Infrastructure.Persistence.EntityPersistence.RoomTypePersistence;
using TravelEase.Infrastructure.Persistence.EntityPersistence.UserPersistence;
using TravelEase.Infrastructure.Persistence.CommonRepositories;
using TravelEase.Infrastructure.Common.Security;
using TravelEase.Infrastructure.Persistence.Services;

namespace TravelEase.Infrastructure.Common.Extensions
{
    public static class InfrastructureServiceExtensions
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {

            var connectionString = configuration.GetConnectionString("TravelEaseDb");

            services.AddDbContext<TravelEaseDbContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddScoped<ICityRepository, CityRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoomAmenityRepository, RoomAmenityRepository>();
            services.AddScoped<IReviewRepository, ReviewRepository>();
            services.AddScoped<IBookingRepository, BookingRepository>();
            services.AddScoped<IRoomRepository, RoomRepository>();
            services.AddScoped<IHotelRepository, HotelRepository>();
            services.AddScoped<IRoomTypeRepository, RoomTypeRepository>();
            services.AddScoped<IDiscountRepository, DiscountRepository>();
            services.AddScoped<IOwnershipValidator, OwnershipValidator>();

            services.AddScoped(typeof(ICrudRepository<>), typeof(GenericCrudRepository<>));
            services.AddScoped(typeof(IReadableRepository<>), typeof(GenericReadableRepository<>));

            services.AddSecurityServices();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }
    }
}