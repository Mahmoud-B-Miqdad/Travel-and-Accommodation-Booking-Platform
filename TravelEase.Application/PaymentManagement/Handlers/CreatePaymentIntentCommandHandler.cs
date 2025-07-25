﻿using MediatR;
using Microsoft.Extensions.Options;
using Stripe;
using TravelEase.Application.PaymentManagement.Commands;
using TravelEase.Domain.Aggregates.Bookings;
using TravelEase.Domain.Aggregates.Payments;
using TravelEase.Domain.Common.Interfaces;
using TravelEase.Domain.Enums;
using TravelEase.Domain.Exceptions;

namespace TravelEase.Application.PaymentManagement.Handlers
{
    public class CreatePaymentIntentCommandHandler : IRequestHandler<CreatePaymentIntentCommand, string>
    {
        private readonly IPaymentService _stripePaymentService;
        private readonly IUnitOfWork _unitOfWork;

        public CreatePaymentIntentCommandHandler
            (IPaymentService stripePaymentService, IUnitOfWork unitOfWork)
        {
            _stripePaymentService = stripePaymentService;
            _unitOfWork = unitOfWork;
        }

        public async Task<string> Handle(CreatePaymentIntentCommand request, CancellationToken cancellationToken)
        {
            var booking = await GetBookingAsync(request.BookingId);
            await EnsureUserCanAccessBooking(request.BookingId, request.GuestEmail);

            var paymentIntent = await _stripePaymentService.CreatePaymentIntentAsync
                (request.BookingId, request.Amount, request.Method);

            var payment = new Payment
            {
                Id = Guid.NewGuid(),
                BookingId = request.BookingId,
                Amount = request.Amount,
                Method = request.Method,
                Status = PaymentStatus.Pending,
                PaymentIntentId = paymentIntent.Id
            };

            await _unitOfWork.Payments.AddAsync(payment);
            await _unitOfWork.SaveChangesAsync();

            return paymentIntent.ClientSecret;
        }

        private async Task<Booking> GetBookingAsync(Guid bookingId)
        {
            var booking = await _unitOfWork.Bookings.GetByIdAsync(bookingId);
            if (booking == null)
                throw new NotFoundException($"Booking with ID {bookingId} was not found.");

            return booking;
        }

        private async Task EnsureUserCanAccessBooking(Guid bookingId, string guestEmail)
        {
            var isAccessible = await _unitOfWork.Bookings.IsBookingAccessibleToUserAsync(bookingId, guestEmail);
            if (!isAccessible)
                throw new UnauthorizedAccessException("You are not authorized to access this booking.");
        }
    }
}