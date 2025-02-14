namespace EraShop.API.Entities
{
	public class OrderItem : AuditableEntity
	{
		public int Id { get; set; }
		public required ProductItemOrdered Product { get; set; }
		public decimal Price { get; set; }
		public int Quantity { get; set; }
	}
}
