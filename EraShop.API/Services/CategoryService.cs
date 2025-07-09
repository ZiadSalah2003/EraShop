using EraShop.API.Abstractions;
using EraShop.API.Contracts.Categories;
using EraShop.API.Contracts.Infrastructure;
using EraShop.API.Entities;
using EraShop.API.Errors;
using EraShop.API.Specification.Category;
using Mapster;

namespace EraShop.API.Services
{
	public class CategoryService : ICategoryService
	{
		private readonly IUnitOfWork _unitOfWork;
		public CategoryService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}
		public async Task<IEnumerable<CategoryResponse>> GetAllAdync(CancellationToken cancellationToken = default)
		{
			var categoryRepository = _unitOfWork.GetRepository<Category, int>();
			var activeCategoriesSpec = new CategorySpecification(true);
			var categories = await categoryRepository.GetAllWithSpecAsync(activeCategoriesSpec);
			
			return categories.Adapt<IEnumerable<CategoryResponse>>();
		}
		public async Task<Result<CategoryResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
		{
			var categoryRepository = _unitOfWork.GetRepository<Category, int>();
			var categorySpec = new CategorySpecification(id);
			var category = await categoryRepository.GetWithSpecAsync(categorySpec);
			
			if (category is null || category.IsDisable)
				return Result.Failure<CategoryResponse>(CategoryErrors.CategoryNotFound);

			return Result.Success(category.Adapt<CategoryResponse>());
		}
		public async Task<Result<CategoryResponse>> AddAsync(CategoryRequest request, CancellationToken cancellationToken = default)
		{
			var categoryRepository = _unitOfWork.GetRepository<Category, int>();
			var nameSpec = new CategorySpecification(request.Name);
			var isExist = await categoryRepository.GetWithSpecAsync(nameSpec);
			
			if (isExist != null)
				return Result.Failure<CategoryResponse>(CategoryErrors.DublicatedName);

			var category = request.Adapt<Category>();
			await categoryRepository.AddAsync(category, cancellationToken);
			await _unitOfWork.CompleteAsync();

			return Result.Success(category.Adapt<CategoryResponse>());
		}
		public async Task<Result> UpdateAsync(int id, CategoryRequest request, CancellationToken cancellationToken = default)
		{
			var categoryRepository = _unitOfWork.GetRepository<Category, int>();
			
			var nameSpec = new CategorySpecification(request.Name);
			var isExist = await categoryRepository.GetWithSpecAsync(nameSpec);
			if (isExist != null && isExist.Id != id)
				return Result.Failure(CategoryErrors.DublicatedName);

			var categorySpec = new CategorySpecification(id);
			var category = await categoryRepository.GetWithSpecAsync(categorySpec);
			if (category is null || category.IsDisable)
				return Result.Failure(CategoryErrors.CategoryNotFound);

			category.Name = request.Name;
			categoryRepository.Update(category);
			await _unitOfWork.CompleteAsync();
			return Result.Success();
		}
		public async Task<Result> ToggleStatus(int id, CancellationToken cancellationToken = default)
		{
			var categoryRepository = _unitOfWork.GetRepository<Category, int>();
			var categorySpec = new CategorySpecification(id);
			var category = await categoryRepository.GetWithSpecAsync(categorySpec);
			
			if (category is null)
				return Result.Failure(CategoryErrors.CategoryNotFound);

			category.IsDisable = !category.IsDisable;
			categoryRepository.Update(category);
			await _unitOfWork.CompleteAsync();
			return Result.Success();
		}
	}
}
