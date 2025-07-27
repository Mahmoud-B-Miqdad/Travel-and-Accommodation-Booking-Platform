using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using SendGrid;
using SendGrid.Helpers.Mail;
using TravelEase.Domain.Common.Models.EmailModels;
using TravelEase.Infrastructure.Persistence.Services.EmailService;

namespace TravelEase.Tests.Infrastructure.UnitTests.EmailServices
{
    public class SendGridEmailServiceTests
    {
        [Fact]
        public async Task SendEmailAsync_ShouldSendEmail_WhenDataIsValid()
        {
            var emailMessage = new EmailMessage(
                new[] { "recipient@test.com" },
                "Test Subject",
                "Plain text body",
                "<p>HTML body</p>"
            );

            var mockClient = new Mock<ISendGridClient>();
            mockClient.Setup(c => c.SendEmailAsync(It.IsAny<SendGridMessage>(), default))
                .ReturnsAsync(new Response(System.Net.HttpStatusCode.Accepted, null, null));

            var configMock = new Mock<IConfiguration>();
            configMock.Setup(c => c["EmailSettings:SenderEmail"]).Returns("sender@test.com");
            configMock.Setup(c => c["EmailSettings:SenderName"]).Returns("TravelEase");

            var loggerMock = new Mock<ILogger<SendGridEmailService>>();

            var service = new SendGridEmailService(mockClient.Object, configMock.Object, loggerMock.Object);

            var act = () => service.SendEmailAsync(emailMessage);

            await act.Should().NotThrowAsync();

            mockClient.Verify(c =>
                c.SendEmailAsync(It.Is<SendGridMessage>(msg =>
                    msg.Subject == emailMessage.Subject &&
                    msg.From.Email == "sender@test.com" &&
                    msg.Personalizations.First().Tos.First().Email == "recipient@test.com"
                ), default), Times.Once);
        }
    }
}