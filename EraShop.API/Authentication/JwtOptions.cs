﻿namespace EraShop.API.Authentication
{
	public class JwtOptions
	{
		public static string SectionName = "Jwt";

		public string Key { get; set; } = string.Empty;
		public string Issuer { get; set; } = string.Empty;
		public string Audience { get; set; } = string.Empty;
		public int ExpiryMinutes { get; init; }
	}
}
