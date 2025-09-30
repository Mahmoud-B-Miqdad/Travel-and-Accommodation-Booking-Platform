namespace TravelEase.Domain.Common.Models.SettingModels
{
    public class StripeSettings
    {
        public string SecretKey { get; set; } = default!;
        public string WebhookSecret { get; set; } = default!;
    }
}