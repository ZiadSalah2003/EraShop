namespace EraShop.API.Entities
{
	public class Review
	{
		public int Id { get; set; }
		public string Comment { get; set; } = string.Empty;
		public string UserId { get; set; } = string.Empty;
		public int? Rating { get; set; }
		public bool IsDisable { get; set; }
		public int? ProductId { get; set; }
		public virtual Product? Product { get; set; }
		public virtual ApplicationUser? User { get; set; }
	}
}
