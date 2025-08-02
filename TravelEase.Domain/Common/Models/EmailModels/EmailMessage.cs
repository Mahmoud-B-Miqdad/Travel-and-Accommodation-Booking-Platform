namespace TravelEase.Domain.Common.Models.EmailModels
{
    public class EmailMessage
    {
        public List<string> To { get; set; } = new();
        public string Subject { get; set; }
        public string PlainTextContent { get; set; }
        public string HtmlContent { get; set; }

        public EmailMessage(IEnumerable<string> to, string subject, string plainText, string html)
        {
            To = to.ToList();
            Subject = subject;
            PlainTextContent = plainText;
            HtmlContent = html;
        }
    }
}