﻿namespace EraShop.API.Entities
{
	public class BasketItem
	{
		public int Id { get; set; }
		public string? ProductName { get; set; }
		public string? ProductImage { get; set; }
		public decimal Price { get; set; }
		public int Quantity { get; set; }
		public string? Brand { get; set; }
		public string? Category { get; set; }
	}
}
