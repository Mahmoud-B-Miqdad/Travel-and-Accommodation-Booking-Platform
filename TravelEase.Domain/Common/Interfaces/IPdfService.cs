namespace TravelEase.Domain.Common.Interfaces
{
    public interface IPdfService
    {
        byte[] CreatePdfFromHtml(string html);
    }
}