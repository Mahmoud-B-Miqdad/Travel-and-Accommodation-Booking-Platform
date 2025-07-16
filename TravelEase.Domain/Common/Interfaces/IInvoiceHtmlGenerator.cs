using TravelEase.Domain.Common.Models.CommonModels;

namespace TravelEase.Domain.Common.Interfaces
{
    public interface IInvoiceHtmlGenerator
    {
        string GenerateHtml(Invoice invoice, string userName);
    }
}