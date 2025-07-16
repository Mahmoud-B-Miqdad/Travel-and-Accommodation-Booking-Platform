using TravelEase.Domain.Common.Interfaces;
using TravelEase.Domain.Common.Models.CommonModels;
using TravelEase.Domain.Common.Models.EmailModels;

namespace TravelEase.Infrastructure.Persistence.Services.EmailService
{
    public class InvoiceEmailBuilder : IInvoiceEmailBuilder
    {
        public EmailMessage CreateInvoiceEmail(Guid bookingId, string email, string name, Invoice invoice)
        {
            var subject = "Your Invoice is Ready!";

            var plainText = $@"
Dear {name},

Your invoice for booking ID: {bookingId} is ready.

Hotel: {invoice.HotelName}
Total: {invoice.Price:C}

Best regards,
{invoice.OwnerName}";

            var html = $@"
<p>Dear {name},</p>
<p>Your invoice for booking ID: <strong>{bookingId}</strong> is ready.</p>
<ul>
  <li><strong>Hotel:</strong> {invoice.HotelName}</li>
  <li><strong>Total:</strong> {invoice.Price:C}</li>
</ul>
<p>Best regards,<br/>{invoice.OwnerName}</p>";

            return new EmailMessage(new[] { email }, subject, plainText.Trim(), html.Trim());
        }
    }
}