using EraShop.API.Contracts.Bills;
using EraShop.API.Errors;
using EraShop.API.Persistence;
using Mapster;

namespace EraShop.API.Services
{
	public class BillService : IBillService
	{
		private readonly ApplicationDbContext _context;

		public BillService(ApplicationDbContext context)
		{
			_context = context;
		}
		public async Task<Result<IEnumerable<BillResponse>>> GetAllBillsAsync()
		{
			var bills = await _context.Bills.ToListAsync();

			var billResponses = bills.Adapt<IEnumerable<BillResponse>>();
			return Result.Success(billResponses);
		}
	}
}
