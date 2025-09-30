using TravelEase.Domain.Common.Models.CommonModels;
using TravelEase.Domain.Common.Models.EmailModels;

namespace TravelEase.Domain.Common.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(EmailMessage message, List<FileAttachment>? attachments = null);
    }
}