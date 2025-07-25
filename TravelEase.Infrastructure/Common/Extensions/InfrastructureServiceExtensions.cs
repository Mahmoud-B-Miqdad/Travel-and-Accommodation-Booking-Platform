﻿using Microsoft.EntityFrameworkCore;
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
using TravelEase.Infrastructure.Persistence.Services.CommonServices;
using TravelEase.Infrastructure.Persistence.Services.PDFServices;
using NReco.PdfGenerator;
using TravelEase.Infrastructure.Persistence.Services.EmailService;
using TravelEase.Infrastructure.Persistence.Services.ImageServices;
using TravelEase.Domain.Aggregates.Images;
using TravelEase.Infrastructure.Persistence.EntityPersistence.ImagePersistence;
using TravelEase.Infrastructure.Persistence.Services.PaymentServices;
using TravelEase.Domain.Aggregates.Payments;
using TravelEase.Infrastructure.Persistence.EntityPersistence.PaymentPersistence;

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
            services.AddScoped<IImageRepository, ImageRepository>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();

            services.AddScoped(typeof(ICrudRepository<>), typeof(GenericCrudRepository<>));
            services.AddScoped(typeof(IReadableRepository<>), typeof(GenericReadableRepository<>));

            services.AddScoped<IInvoiceHtmlGenerator, InvoiceHtmlGenerator>();
            services.AddScoped<IPdfService, PdfService>();
            services.AddScoped<IInvoiceEmailBuilder, InvoiceEmailBuilder>();
            services.AddScoped<IEmailService, SendGridEmailService>();
            services.AddScoped<HtmlToPdfConverter>();
            services.AddScoped<IImageService, CloudinaryImageService>();
            services.AddScoped<IPaymentService, StripePaymentService>();

            services.AddSecurityServices();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}