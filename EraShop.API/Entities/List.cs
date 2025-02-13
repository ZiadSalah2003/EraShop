namespace EraShop.API.Entities
{
    public class List
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public ApplicationUser User { get; set; } = default!;
        public ICollection<ListItem> Items { get; set; } = [];
    }
}
