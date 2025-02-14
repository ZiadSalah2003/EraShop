namespace EraShop.API.Settings
{
	public class StripeSettings
	{
		public required string SecretKey { get; set; }
		public required string WebhookSecret { get; set; }

	}
}
