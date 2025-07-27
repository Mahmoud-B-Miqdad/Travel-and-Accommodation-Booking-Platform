using FluentAssertions;
using TravelEase.Domain.Common.Models.CommonModels;
using TravelEase.Infrastructure.Persistence.Services.EmailService;

namespace TravelEase.Tests.Infrastructure.UnitTests.EmailServices
{
    public class InvoiceEmailBuilderTests
    {
        [Fact]
        public void CreateInvoiceEmail_ShouldReturnEmailMessage_WithCorrectContent()
        {
            var builder = new InvoiceEmailBuilder();
            var bookingId = Guid.NewGuid();
            var email = "guest@example.com";
            var name = "John Doe";
            var invoice = new Invoice
            {
                Id = bookingId,
                HotelName = "Grand Palace",
                Price = 250.0,
                OwnerName = "Hotel Owner"
            };

            var result = builder.CreateInvoiceEmail(bookingId, email, name, invoice);

            result.Should().NotBeNull();
            result.To.Should().ContainSingle().Which.Should().Be(email);
            result.Subject.Should().Be("Your Invoice is Ready!");
            result.PlainTextContent.Should().Contain(name)
                                 .And.Contain(invoice.HotelName)
                                 .And.Contain(invoice.OwnerName)
                                 .And.Contain(invoice.Price.ToString("C"));
            result.HtmlContent.Should().Contain("<p>Dear")
                            .And.Contain(invoice.HotelName)
                            .And.Contain(invoice.Price.ToString("C"))
                            .And.Contain(invoice.OwnerName);
        }
    }
}