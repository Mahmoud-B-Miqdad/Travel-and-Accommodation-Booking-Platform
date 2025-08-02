using FluentAssertions;
using TravelEase.Domain.Common.Models.CommonModels;
using TravelEase.Infrastructure.Persistence.Services.PDFServices;

namespace TravelEase.Tests.Infrastructure.UnitTests.PDFServices
{
    public class InvoiceHtmlGeneratorTests
    {
        [Fact]
        public void GenerateHtml_ShouldIncludeInvoiceDetailsInHtml()
        {
            var invoice = new Invoice
            {
                BookingId = Guid.NewGuid(),
                BookingDate = new DateTime(2025, 7, 27),
                HotelName = "Grand Palace",
                Price = 250
            };
            var userName = "John Doe";
            var generator = new InvoiceHtmlGenerator();

            var html = generator.GenerateHtml(invoice, userName);

            html.Should().NotBeNullOrWhiteSpace();
            html.Should().Contain(invoice.BookingId.ToString());
            html.Should().Contain("2025/07/27");
            html.Should().Contain("Grand Palace");
            html.Should().Contain("250");
            html.Should().Contain(userName);
            html.Should().Contain("<html"); 
        }
    }
}