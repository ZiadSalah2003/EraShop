using Microsoft.AspNetCore.Http;

namespace EraShop.API.Abstractions.Consts
{
	public static class DefaultDeliveryMethod
	{
		public const int Id1 = 1;
		public const string ShortName1 = "UPS1";
		public const string Description1 = "Fastest delivery time";
		public const decimal Cost1 = 10;
		public const string DeliveryTime1 = "1-2 Days";

		public const int Id2 = 2;
		public const string ShortName2 = "UPS2";
		public const string Description2 = "Get it within 5 days";
		public const decimal Cost2 = 5;
		public const string DeliveryTime2 = "2-5 Days";

		public const int Id3 = 3;
		public const string ShortName3 = "UPS3";
		public const string Description3 = "Slower but cheap";
		public const decimal Cost3 = 2;
		public const string DeliveryTime3 = "5-10 Days";

		public const int Id = 4;
		public const string ShortName = "Free";
		public const string Description = "Free! you get what you pay for";
		public const decimal Cost = 0;
		public const string DeliveryTime = "1-2 Weeks";
	}
}
