﻿namespace EraShop.API.Entities
{
	public class CustomerBasket
	{
		public string Id { get; set; }
		public IEnumerable<BasketItem> BasketItem { get; set; } = [];
	}
}
