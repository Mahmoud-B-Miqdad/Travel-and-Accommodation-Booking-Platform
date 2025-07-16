using NReco.PdfGenerator;
using TravelEase.Domain.Common.Interfaces;

namespace TravelEase.Infrastructure.Persistence.Services.PDFServices
{
    public class PdfService : IPdfService
    {
        private readonly HtmlToPdfConverter _converter;

        public PdfService(HtmlToPdfConverter converter)
        {
            _converter = converter;
        }

        public byte[] CreatePdfFromHtml(string html)
        {
            return _converter.GeneratePdf(html);
        }
    }

}