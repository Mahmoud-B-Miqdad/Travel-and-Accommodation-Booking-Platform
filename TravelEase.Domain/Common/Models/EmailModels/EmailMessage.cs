namespace TravelEase.Domain.Common.Models.EmailModels
{
    public class EmailMessage
    {
        public List<string> To { get; set; } = new();
        public string Subject { get; set; } = string.Empty;
        public string PlainTextContent { get; set; } = string.Empty;
        public string HtmlContent { get; set; } = string.Empty;

        public EmailMessage(IEnumerable<string> to, string subject, string plainText, string html)
        {
            To = to.ToList();
            Subject = subject;
            PlainTextContent = plainText;
            HtmlContent = html;
        }
    }
}