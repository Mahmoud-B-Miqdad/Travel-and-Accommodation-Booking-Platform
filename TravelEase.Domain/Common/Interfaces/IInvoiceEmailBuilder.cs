using TravelEase.Domain.Common.Models.CommonModels;
using TravelEase.Domain.Common.Models.EmailModels;

namespace TravelEase.Domain.Common.Interfaces
{
    public interface IInvoiceEmailBuilder
    {
        EmailMessage CreateInvoiceEmail(Guid bookingId, string email, string name, Invoice invoice);
    }
}