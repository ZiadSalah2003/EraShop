using EraShop.API.Abstractions;
using EraShop.API.Contracts.Categories;

namespace EraShop.API.Services
{
    public interface ICategoryService
    {
		public Task<Result> ToggleStatus(int id, CancellationToken cancellationToken = default);
		public Task<Result> UpdateAsync(int id, CategoryRequest request, CancellationToken cancellationToken = default);
		public Task<Result<CategoryResponse>> AddAsync(CategoryRequest request, CancellationToken cancellationToken = default);
		public Task<Result<CategoryResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default);
		public Task<IEnumerable<CategoryResponse>> GetAllAdync(CancellationToken cancellationToken = default);


	}
}
