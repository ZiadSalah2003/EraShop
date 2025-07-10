using EraShop.API.Abstractions;
using EraShop.API.Abstractions.Consts;
using EraShop.API.Contracts.Common;
using EraShop.API.Contracts.Infrastructure;
using EraShop.API.Contracts.Products;
using EraShop.API.Entities;
using EraShop.API.Errors;
using EraShop.API.Specification.Brand;
using EraShop.API.Specification.Category;
using EraShop.API.Specification.Product;
using Mapster;
using System.Linq.Dynamic.Core;

namespace EraShop.API.Services
{
	public class ProductService : IProductService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IFileService _fileService;
		private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly INotificationService _notificationService;
		public ProductService(IUnitOfWork unitOfWork, IFileService fileService, IHttpContextAccessor httpContextAccessor , INotificationService notificationService)
		{
			_unitOfWork = unitOfWork;
			_fileService = fileService;
			_httpContextAccessor = httpContextAccessor;
            _notificationService = notificationService;
		}
		public async Task<PaginatedList<ProductResponse>> GetAllAdync(RequestFilters filters, CancellationToken cancellationToken = default)
		{
			var productRepository = _unitOfWork.GetRepository<EraShop.API.Entities.Product, int>();
			var activeProductsSpec = new ProductSpecification(true);
			var products = await productRepository.GetAllWithSpecAsync(activeProductsSpec);
			
			var filteredProducts = products.AsQueryable();
			
			if (!string.IsNullOrEmpty(filters.SearchValue))
				filteredProducts = filteredProducts.Where(x => x.Name.Contains(filters.SearchValue));

			if (!string.IsNullOrEmpty(filters.SortColumn))
				filteredProducts = filteredProducts.OrderBy($"{filters.SortColumn} {filters.SortDirection}");

			var productResponses = filteredProducts.Adapt<IQueryable<ProductResponse>>();
			var response = await PaginatedList<ProductResponse>.CreateAsync(productResponses, filters.PageNumber, filters.PageSize, cancellationToken);
			return response;
		}
		public async Task<Result<ProductResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
		{
			var productRepository = _unitOfWork.GetRepository<EraShop.API.Entities.Product, int>();
			var productSpec = new ProductSpecification(id);
			var product = await productRepository.GetWithSpecAsync(productSpec);

			if (product is null || product.IsDisable)
				return Result.Failure<ProductResponse>(ProductErrors.ProductNotFound);

			return Result.Success(product.Adapt<ProductResponse>());
		}
		public async Task<Result<ProductResponse>> AddAsync(ProductRequest request, CancellationToken cancellationToken = default)
		{
			var productRepository = _unitOfWork.GetRepository<EraShop.API.Entities.Product, int>();
			var brandRepository = _unitOfWork.GetRepository<EraShop.API.Entities.Brand, int>();
			var categoryRepository = _unitOfWork.GetRepository<EraShop.API.Entities.Category, int>();
			
			var nameSpec = new ProductSpecification(request.Name);
			var isExist = await productRepository.GetWithSpecAsync(nameSpec);
			if (isExist != null)
				return Result.Failure<ProductResponse>(ProductErrors.DublicatedName);

			var brandSpec = new BrandSpecification(request.BrandId);
			var brand = await brandRepository.GetWithSpecAsync(brandSpec);
			if (brand is null)
				return Result.Failure<ProductResponse>(BrandErrors.BrandNotFound);

			var categorySpec = new CategorySpecification(request.CategoryId);
			var category = await categoryRepository.GetWithSpecAsync(categorySpec);
			if (category is null)
				return Result.Failure<ProductResponse>(CategoryErrors.CategoryNotFound);

			var uploadResult = await _fileService.UploadToCloudinaryAsync(request.ImageUrl!);
			if (!uploadResult.IsSuccess)
				return Result.Failure<ProductResponse>(FileErrors.UploadFailed);
			
			var product = request.Adapt<EraShop.API.Entities.Product>();
			product.ImageUrl = uploadResult.Value.SecureUrl;

			await productRepository.AddAsync(product, cancellationToken);
			await _unitOfWork.CompleteAsync();

			await _notificationService.SendNewProductsNotifications(product);
			return Result.Success(product.Adapt<ProductResponse>());
		}
		public async Task<Result> UpdateAsync(int id, ProductRequest request, CancellationToken cancellationToken = default)
		{
			var productRepository = _unitOfWork.GetRepository<EraShop.API.Entities.Product, int>();
			
			var nameSpec = new ProductSpecification(request.Name);
			var isExist = await productRepository.GetWithSpecAsync(nameSpec);
			if (isExist != null && isExist.Id != id)
				return Result.Failure(ProductErrors.DublicatedName);

			var productSpec = new ProductSpecification(id);
			var product = await productRepository.GetWithSpecAsync(productSpec);
			if (product is null || product.IsDisable)
				return Result.Failure(ProductErrors.ProductNotFound);

			product.Name = request.Name;
			product.Description = request.Description;
			product.Price = request.Price;
			product.Quantity = request.Quantity;
			product.BrandId = request.BrandId;
			product.CategoryId = request.CategoryId;

			if (request.ImageUrl != null)
			{
				if (!string.IsNullOrEmpty(product.ImageUrl))
				{
					var publicId = _fileService.ExtractPublicIdFromUrl(product.ImageUrl);
					if (!string.IsNullOrEmpty(publicId))
						await _fileService.DeleteFromCloudinaryAsync(publicId);
				}
				var uploadResult = await _fileService.UploadToCloudinaryAsync(request.ImageUrl);
				if (!uploadResult.IsSuccess)
					return Result.Failure(FileErrors.UploadFailed);

				product.ImageUrl = uploadResult.Value.SecureUrl;
			}

			productRepository.Update(product);
			await _unitOfWork.CompleteAsync();
			return Result.Success();
		}
		public async Task<Result> ToggleStatus(int id, CancellationToken cancellationToken = default)
		{
			var productRepository = _unitOfWork.GetRepository<EraShop.API.Entities.Product, int>();
			var productSpec = new ProductSpecification(id);
			var product = await productRepository.GetWithSpecAsync(productSpec);
			
			if (product is null)
				return Result.Failure(ProductErrors.ProductNotFound);

			product.IsDisable = !product.IsDisable;
			productRepository.Update(product);
			await _unitOfWork.CompleteAsync();
			return Result.Success();
		}
	}
}
