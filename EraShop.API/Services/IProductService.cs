using EraShop.API.Abstractions;
using EraShop.API.Contracts.Products;

namespace EraShop.API.Services
{
    public interface IProductService
    {
		public Task<IEnumerable<ProductResponse>> GetAllAdync(CancellationToken cancellationToken = default);
		public Task<Result<ProductResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default);
		public Task<Result<ProductResponse>> AddAsync(ProductRequest request, CancellationToken cancellationToken = default);
		public Task<Result> UpdateAsync(int id, ProductRequest request, CancellationToken cancellationToken = default);
		public Task<Result> ToggleStatus(int id, CancellationToken cancellationToken = default);

	}
}
