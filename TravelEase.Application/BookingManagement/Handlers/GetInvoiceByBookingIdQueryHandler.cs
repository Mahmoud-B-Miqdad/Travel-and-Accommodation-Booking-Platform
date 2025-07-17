using AutoMapper;
using MediatR;
using TravelEase.Application.BookingManagement.Queries;
using TravelEase.Domain.Common.Interfaces;
using TravelEase.Domain.Common.Models.CommonModels;
using TravelEase.Domain.Exceptions;

namespace TravelEase.Application.BookingManagement.Handlers
{
    public class GetInvoiceByBookingIdQueryHandler 
        : IRequestHandler<GetInvoiceByBookingIdQuery, InvoiceResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IInvoiceHtmlGenerator _invoiceHtmlGenerator;
        private readonly IPdfService _pdfService;
        private readonly IInvoiceEmailBuilder _invoiceEmailBuilder;
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;

        public GetInvoiceByBookingIdQueryHandler(IUnitOfWork unitOfWork,
            IInvoiceHtmlGenerator invoiceHtmlGenerator, IPdfService pdfService
            , IInvoiceEmailBuilder invoiceEmailBuilder, IEmailService emailService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _invoiceHtmlGenerator = invoiceHtmlGenerator;
            _pdfService = pdfService;
            _invoiceEmailBuilder = invoiceEmailBuilder;
            _emailService = emailService;
            _mapper = mapper;
        }

        public async Task<InvoiceResponse> Handle
            (GetInvoiceByBookingIdQuery request, CancellationToken cancellationToken)
        {
            await EnsureHotelExistsAsync(request.HotelId);
            await EnsureBookingExists(request.BookingId);

            var invoice = await _unitOfWork.Bookings.GetInvoiceByBookingIdAsync(request.BookingId);
            ThrowIfInvoiceNotFound(invoice);

            await EnsureUserCanAccessBooking(request.BookingId, request.GuestEmail);

            byte[]? pdfBytes = null;

            if (request.SendByEmail || request.GeneratePdf)
            {
                var html = _invoiceHtmlGenerator.GenerateHtml(invoice, request.GuestName);
                pdfBytes = _pdfService.CreatePdfFromHtml(html);
            }

            if (request.SendByEmail)
            {
                var message = _invoiceEmailBuilder.CreateInvoiceEmail(
                    request.BookingId, request.GuestEmail, request.GuestName, invoice);

                var attachments = new List<FileAttachment>
                {
                    new("invoice.pdf", pdfBytes!, "application/pdf")
                };

                await _emailService.SendEmailAsync(message, attachments);
            }


            var response = _mapper.Map<InvoiceResponse>(invoice);
            return response;
        }

        private async Task EnsureHotelExistsAsync(Guid hotelId)
        {
            if (!await _unitOfWork.Hotels.ExistsAsync(hotelId))
                throw new NotFoundException($"Hotel with ID {hotelId} doesn't exist.");
        }

        private async Task EnsureBookingExists(Guid bookingId)
        {
            if (!await _unitOfWork.Bookings.ExistsAsync(bookingId))
                throw new NotFoundException("Booking doesn't exist.");
        }

        private static void ThrowIfInvoiceNotFound(Invoice? invoice)
        {
            if (invoice is null)
                throw new NotFoundException($"Invoice not found.");
        }

        private async Task EnsureUserCanAccessBooking(Guid bookingId, string guestEmail)
        {
            var isAccessible = await _unitOfWork.Bookings.IsBookingAccessibleToUserAsync(bookingId, guestEmail);
            if (!isAccessible)
                throw new NotFoundException("You do not have access to this booking");
        }
    }
}