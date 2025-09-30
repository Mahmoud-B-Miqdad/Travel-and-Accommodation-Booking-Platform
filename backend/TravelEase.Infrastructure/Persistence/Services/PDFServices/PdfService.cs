using DinkToPdf;
using DinkToPdf.Contracts;
using TravelEase.Domain.Common.Interfaces;

namespace TravelEase.Infrastructure.Persistence.Services.PDFServices
{
    public class PdfService : IPdfService
    {
        private readonly IConverter _converter;

        public PdfService(IConverter converter)
        {
            _converter = converter;
        }

        public byte[] CreatePdfFromHtml(string html)
        {
            var doc = new HtmlToPdfDocument()
            {
                GlobalSettings = {
                    PaperSize = PaperKind.A4,
                    Orientation = Orientation.Portrait,
                    Margins = new MarginSettings { Top = 10, Bottom = 10, Left = 10, Right = 10 }
                },
                Objects = {
                    new ObjectSettings()
                    {
                        HtmlContent = html
                    }
                }
            };

            return _converter.Convert(doc);
        }
    }
}