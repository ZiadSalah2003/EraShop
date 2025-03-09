using EraShop.API.Contracts.Bills;

namespace EraShop.API.Services
{
	public interface IBillService
	{
		Task<Result<IEnumerable<BillResponse>>> GetAllBillsAsync();
	}
}
