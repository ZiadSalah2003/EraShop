namespace EraShop.API.Entities
{
	public class Product : AuditableEntity
	{
		public int Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
		public decimal Price { get; set; }
		public string? ImageUrl { get; set; } = string.Empty;
		public int Quantity { get; set; }
		public bool IsDisable { get; set; }
		public int? BrandId { get; set; }
		public int? CategoryId { get; set; }
		public virtual Category? Category { get; set; }
		public virtual Brand? Brand { get; set; }
	}
}
