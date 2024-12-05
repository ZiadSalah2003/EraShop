namespace EraShop.API.Contracts.Categories
{
	public record CategoryResponse(
		int Id,
		string Name,
		bool IsDisable
	);
}
