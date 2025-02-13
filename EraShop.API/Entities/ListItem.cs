namespace EraShop.API.Entities
{
    public class ListItem
    {
        public int Id { get; set; }
        public int ListId { get; set; }
        public List List { get; set; } = default!;
        public int ProductId { get; set; }
        public Product Product { get; set; } = default!;
    }
}
