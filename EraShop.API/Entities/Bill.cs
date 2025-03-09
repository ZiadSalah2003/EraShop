namespace EraShop.API.Entities
{
	public class Bill
	{
		public int Id { get; set; }
		public string BuyerName { get; set; } = string.Empty;
		public string BuyerEmail { get; set; } = string.Empty;
		public decimal Subtotal { get; set; }
		public OrderStatus Status { get; set; } = OrderStatus.Pending;
	}
}
