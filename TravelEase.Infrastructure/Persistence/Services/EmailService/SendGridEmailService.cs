using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SendGrid;
using SendGrid.Helpers.Mail;
using TravelEase.Domain.Common.Interfaces;
using TravelEase.Domain.Common.Models.CommonModels;
using TravelEase.Domain.Common.Models.EmailModels;

namespace TravelEase.Infrastructure.Persistence.Services.EmailService
{
    public class SendGridEmailService : IEmailService
    {
        private readonly ISendGridClient _sendGridClient;
        private readonly string _senderEmail;
        private readonly string _senderName;
        private readonly ILogger<SendGridEmailService> _logger;

        public SendGridEmailService(
            ISendGridClient sendGridClient,
            IConfiguration configuration,
            ILogger<SendGridEmailService> logger)
        {
            _sendGridClient = sendGridClient;
            _logger = logger;

            _senderEmail = configuration["EmailSettings:SenderEmail"]
                ?? throw new ArgumentNullException("SenderEmail is missing.");
            _senderName = configuration["EmailSettings:SenderName"] ?? "TravelEase";
        }

        public async Task SendEmailAsync(EmailMessage message, List<FileAttachment>? attachments = null)
        {
            var from = new EmailAddress(_senderEmail, _senderName);
            var tos = message.To.Select(email => new EmailAddress(email)).ToList();

            var msg = MailHelper.CreateSingleEmailToMultipleRecipients(
                from,
                tos,
                message.Subject,
                message.PlainTextContent,
                message.HtmlContent
            );

            if (attachments != null && attachments.Any())
            {
                foreach (var attachment in attachments)
                {
                    msg.AddAttachment(
                        attachment.FileName,
                        Convert.ToBase64String(attachment.Data),
                        attachment.ContentType
                    );
                }
            }

            var response = await _sendGridClient.SendEmailAsync(msg);

            if (!response.IsSuccessStatusCode)
            {
                var body = await response.Body.ReadAsStringAsync();
                _logger.LogError("SendGrid failed: {StatusCode}, Body: {Body}", response.StatusCode, body);
                throw new Exception("Failed to send email.");
            }

            _logger.LogInformation("Email sent successfully to: {Recipients}", string.Join(", ", message.To));
        }
    }
}