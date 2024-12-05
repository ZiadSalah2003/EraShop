namespace EraShop.API.Entities
{
	public class Brand : AuditableEntity
	{
		public int Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public bool IsDisable { get; set; }
        public string? ImageUrl { get; set; } = string.Empty;
        public ICollection<Product> Products { get; set; } = [];
	}
}
