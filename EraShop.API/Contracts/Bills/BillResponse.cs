using EraShop.API.Entities;

namespace EraShop.API.Contracts.Bills
{
	public record BillResponse
	(
		int id,
		string BuyerName,
		string BuyerEmail,
		decimal Subtotal,
		OrderStatus Status
	);
}
