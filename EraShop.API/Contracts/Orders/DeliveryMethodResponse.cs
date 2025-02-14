namespace EraShop.API.Contracts.Orders
{
	public record DeliveryMethodResponse
	(
		int Id,
		string ShortName,
		string Description,
		decimal Cost,
		string DeliveryTime
	);
}
