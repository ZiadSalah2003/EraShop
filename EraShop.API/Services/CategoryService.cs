using EraShop.API.Abstractions;
using EraShop.API.Contracts.Categories;
using EraShop.API.Entities;
using EraShop.API.Errors;
using EraShop.API.Persistence;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace EraShop.API.Services
{
	public class CategoryService : ICategoryService
	{
		private readonly ApplicationDbContext _context;
		public CategoryService(ApplicationDbContext context)
		{
			_context = context;
		}
		public async Task<IEnumerable<CategoryResponse>> GetAllAdync(CancellationToken cancellationToken = default)
		{
			var categories = await _context.Categories
							.Where(c => !c.IsDisable)
							.ProjectToType<CategoryResponse>()
							.ToListAsync(cancellationToken);
			return categories;
		}
		public async Task<Result<CategoryResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
		{
			var isExist = await _context.Categories.AnyAsync(c => c.Id == id && !c.IsDisable, cancellationToken);
			if (!isExist)
				return Result.Failure<CategoryResponse>(CategoryErrors.CategoryNotFound);

			var category = await _context.Categories
							.Where(c => c.Id == id)
							.ProjectToType<CategoryResponse>()
							.FirstOrDefaultAsync(cancellationToken);
			return Result.Success(category!);
		}
		public async Task<Result<CategoryResponse>> AddAsync(CategoryRequest request, CancellationToken cancellationToken = default)
		{
			var isExist = await _context.Categories.AnyAsync(c => c.Name == request.Name, cancellationToken);
			if (isExist)
				return Result.Failure<CategoryResponse>(CategoryErrors.DublicatedName);


			var category = request.Adapt<Category>();
			//category.IsDisable = false;
			//category.Name = request.Name;
			await _context.Categories.AddAsync(category, cancellationToken);
			await _context.SaveChangesAsync(cancellationToken);

			return Result.Success(category.Adapt<CategoryResponse>());
		}
		public async Task<Result> UpdateAsync(int id, CategoryRequest request, CancellationToken cancellationToken = default)
		{
			var isExist = await _context.Categories.AnyAsync(c => c.Name == request.Name && c.Id != id, cancellationToken);
			if (isExist)
				return Result.Failure(CategoryErrors.DublicatedName);

			var isDisable = await _context.Categories.AnyAsync(c=> c.Id == id && !c.IsDisable, cancellationToken);
			if (!isDisable)
				return Result.Failure(CategoryErrors.CategoryNotFound);

			var updatedCategory = await _context.Categories.FindAsync(id, cancellationToken);
			if (updatedCategory is null)
				return Result.Failure(CategoryErrors.CategoryNotFound);

			//updatedCategory.Name = request.Name;
			var category = updatedCategory.Adapt<Category>();
			await _context.SaveChangesAsync(cancellationToken);
			return Result.Success();
		}
		public async Task<Result> ToggleStatus(int id, CancellationToken cancellationToken = default)
		{
			var category = await _context.Categories.FindAsync(id, cancellationToken);
			if (category is null)
				return Result.Failure(CategoryErrors.CategoryNotFound);

			category.IsDisable = !category.IsDisable;
			_context.Categories.Update(category);
			await _context.SaveChangesAsync(cancellationToken);
			return Result.Success();
		}
	}
}
