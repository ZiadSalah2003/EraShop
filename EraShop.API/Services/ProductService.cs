using EraShop.API.Abstractions;
using EraShop.API.Abstractions.Consts;
using EraShop.API.Contracts.Products;
using EraShop.API.Entities;
using EraShop.API.Errors;
using EraShop.API.Persistence;
using Mapster;

namespace EraShop.API.Services
{
	public class ProductService : IProductService
	{
		private readonly ApplicationDbContext _context;
		private readonly IFileService _fileService;
		private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly INotificationService _notificationService;
		public ProductService(ApplicationDbContext context, IFileService fileService, IHttpContextAccessor httpContextAccessor , INotificationService notificationService)
		{
			_context = context;
			_fileService = fileService;
			_httpContextAccessor = httpContextAccessor;
      _notificationService = notificationService;

		}
		public async Task<IEnumerable<ProductResponse>> GetAllAdync(CancellationToken cancellationToken = default)
		{
			var products = await _context.Products
							.Where(c => !c.IsDisable)
							.Include(p => p.Brand)
							.Include(p => p.Category)
							.ProjectToType<ProductResponse>()
							.ToListAsync(cancellationToken);
			return products;
		}
		public async Task<Result<ProductResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
		{
			var isExist = await _context.Products.AnyAsync(c => c.Id == id && !c.IsDisable, cancellationToken);
			if (!isExist)
				return Result.Failure<ProductResponse>(ProductErrors.ProductNotFound);

			var product = await _context.Products
							.Where(c => c.Id == id)
							.Include(p => p.Brand)
							.Include(p => p.Category)
							.ProjectToType<ProductResponse>()
							.FirstOrDefaultAsync(cancellationToken);
			return Result.Success(product!);
		}
		public async Task<Result<ProductResponse>> AddAsync(ProductRequest request, CancellationToken cancellationToken = default)
		{
			var isExist = await _context.Products.AnyAsync(c => c.Name == request.Name, cancellationToken);
			if (isExist)
				return Result.Failure<ProductResponse>(ProductErrors.DublicatedName);

			var brand = await _context.Brands.FindAsync(request.BrandId, cancellationToken);
			if (brand is null)
				return Result.Failure<ProductResponse>(BrandErrors.BrandNotFound);

			var category = await _context.Categories.FindAsync(request.CategoryId, cancellationToken);
			if (category is null)
				return Result.Failure<ProductResponse>(CategoryErrors.CategoryNotFound);

			string createdImageName = await _fileService.SaveFileAsync(request.ImageUrl!, ImageSubFolder.Product);
			var baseUrl = GetBaseUrl();
			var product = request.Adapt<Product>();
			product.ImageUrl = $"{baseUrl}{createdImageName}";

			await _context.Products.AddAsync(product, cancellationToken);
			await _context.SaveChangesAsync(cancellationToken);

			await _notificationService.SendNewProductsNotifications(product);
			return Result.Success(product.Adapt<ProductResponse>());
		}
		public async Task<Result> UpdateAsync(int id, ProductRequest request, CancellationToken cancellationToken = default)
		{
			var isExist = await _context.Products.AnyAsync(c => c.Name == request.Name && c.Id != id, cancellationToken);

			if (isExist)
				return Result.Failure(ProductErrors.DublicatedName);

			var isDisable = await _context.Products.AnyAsync(c => c.Id == id && !c.IsDisable, cancellationToken);
			if (!isDisable)
				return Result.Failure(ProductErrors.ProductNotFound);

			var updatedProduct = await _context.Products.FindAsync(id, cancellationToken);
			if (updatedProduct is null)
				return Result.Failure(ProductErrors.ProductNotFound);

			var product = updatedProduct.Adapt<Product>();
			await _context.SaveChangesAsync(cancellationToken);
			return Result.Success();
		}
		public async Task<Result> ToggleStatus(int id, CancellationToken cancellationToken = default)
		{
			var product = await _context.Products.FindAsync(id, cancellationToken);
			if (product is null)
				return Result.Failure(ProductErrors.ProductNotFound);

			product.IsDisable = !product.IsDisable;
			_context.Products.Update(product);
			await _context.SaveChangesAsync(cancellationToken);
			return Result.Success();
		}
		private string GetBaseUrl()
		{
			var request = _httpContextAccessor.HttpContext?.Request;
			if (request == null)
				throw new InvalidOperationException("HttpContext is not available.");

			return $"{request.Scheme}://{request.Host}/images/Product/";
		}
	}
}
