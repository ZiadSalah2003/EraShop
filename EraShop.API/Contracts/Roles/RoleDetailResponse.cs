namespace EraShop.API.Contracts.Roles
{
	public record RoleDetailResponse(
		string Id,
		string Name,
		bool IsDeleted
	);
}
