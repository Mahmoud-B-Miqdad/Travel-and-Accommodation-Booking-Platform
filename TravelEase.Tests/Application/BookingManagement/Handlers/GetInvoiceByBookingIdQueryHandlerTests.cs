using AutoFixture;
using AutoMapper;
using FluentAssertions;
using Moq;
using TravelEase.Application.BookingManagement.Handlers;
using TravelEase.Application.BookingManagement.Queries;
using TravelEase.Domain.Common.Interfaces;
using TravelEase.Domain.Common.Models.CommonModels;
using TravelEase.Domain.Common.Models.EmailModels;
using TravelEase.Domain.Exceptions;

namespace TravelEase.Tests.Application.BookingManagement.Handlers
{
    public class GetInvoiceByBookingIdQueryHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
        private readonly Mock<IInvoiceHtmlGenerator> _htmlGeneratorMock = new();
        private readonly Mock<IPdfService> _pdfServiceMock = new();
        private readonly Mock<IInvoiceEmailBuilder> _emailBuilderMock = new();
        private readonly Mock<IEmailService> _emailServiceMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly GetInvoiceByBookingIdQueryHandler _handler;
        private readonly Fixture _fixture = new();

        public GetInvoiceByBookingIdQueryHandlerTests()
        {
            _handler = new GetInvoiceByBookingIdQueryHandler(
                _unitOfWorkMock.Object,
                _htmlGeneratorMock.Object,
                _pdfServiceMock.Object,
                _emailBuilderMock.Object,
                _emailServiceMock.Object,
                _mapperMock.Object
            );
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFound_WhenHotelDoesNotExist()
        {
            var query = _fixture.Create<GetInvoiceByBookingIdQuery>();

            _unitOfWorkMock.Setup(u => u.Hotels.ExistsAsync(query.HotelId))
                .ReturnsAsync(false);

            Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);

            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage($"Hotel with ID {query.HotelId} doesn't exist.");
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFound_WhenBookingDoesNotExist()
        {
            var query = _fixture.Create<GetInvoiceByBookingIdQuery>();

            _unitOfWorkMock.Setup(u => u.Hotels.ExistsAsync(query.HotelId)).ReturnsAsync(true);
            _unitOfWorkMock.Setup(u => u.Bookings.ExistsAsync(query.BookingId)).ReturnsAsync(false);

            Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);

            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage("Booking doesn't exist.");
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFound_WhenInvoiceIsNull()
        {
            var query = _fixture.Create<GetInvoiceByBookingIdQuery>();

            _unitOfWorkMock.Setup(u => u.Hotels.ExistsAsync(query.HotelId)).ReturnsAsync(true);
            _unitOfWorkMock.Setup(u => u.Bookings.ExistsAsync(query.BookingId)).ReturnsAsync(true);
            _unitOfWorkMock.Setup(u => u.Bookings.GetInvoiceByBookingIdAsync(query.BookingId))
                .ReturnsAsync((Invoice?)null);

            Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);

            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage("Invoice not found.");
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFound_WhenUserCannotAccessBooking()
        {
            var query = _fixture.Create<GetInvoiceByBookingIdQuery>();
            var invoice = _fixture.Create<Invoice>();

            _unitOfWorkMock.Setup(u => u.Hotels.ExistsAsync(query.HotelId)).ReturnsAsync(true);
            _unitOfWorkMock.Setup(u => u.Bookings.ExistsAsync(query.BookingId)).ReturnsAsync(true);
            _unitOfWorkMock.Setup(u => u.Bookings.GetInvoiceByBookingIdAsync(query.BookingId))
                .ReturnsAsync(invoice);
            _unitOfWorkMock.Setup(u => u.Bookings.IsBookingAccessibleToUserAsync
            (query.BookingId, query.GuestEmail)).ReturnsAsync(false);

            Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);

            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage("You do not have access to this booking");
        }

        [Fact]
        public async Task Handle_ShouldReturnInvoiceResponse_WhenSuccessWithoutPdfOrEmail()
        {
            var query = _fixture.Build<GetInvoiceByBookingIdQuery>()
                .With(q => q.SendByEmail, false)
                .With(q => q.GeneratePdf, false)
                .Create();

            var invoice = _fixture.Create<Invoice>();
            var response = _fixture.Create<InvoiceResponse>();

            _unitOfWorkMock.Setup(u => u.Hotels.ExistsAsync(query.HotelId)).ReturnsAsync(true);
            _unitOfWorkMock.Setup(u => u.Bookings.ExistsAsync(query.BookingId)).ReturnsAsync(true);
            _unitOfWorkMock.Setup(u => u.Bookings.GetInvoiceByBookingIdAsync(query.BookingId))
                .ReturnsAsync(invoice);

            _unitOfWorkMock.Setup(u => u.Bookings.IsBookingAccessibleToUserAsync
            (query.BookingId, query.GuestEmail)).ReturnsAsync(true);

            _mapperMock.Setup(m => m.Map<InvoiceResponse>(invoice)).Returns(response);

            var result = await _handler.Handle(query, CancellationToken.None);

            result.Should().BeEquivalentTo(response);
            _pdfServiceMock.Verify(p => p.CreatePdfFromHtml(It.IsAny<string>()), Times.Never);
            _emailServiceMock.Verify(e => e.SendEmailAsync(It.IsAny<EmailMessage>(),
                It.IsAny<List<FileAttachment>>()), Times.Never);
        }
    }
}