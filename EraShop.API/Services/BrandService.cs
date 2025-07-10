using EraShop.API.Abstractions;
using EraShop.API.Abstractions.Consts;
using EraShop.API.Contracts.Brands;
using EraShop.API.Contracts.Infrastructure;
using EraShop.API.Entities;
using EraShop.API.Errors;
using EraShop.API.Specification.Brand;
using Mapster;

namespace EraShop.API.Services
{
    public class BrandService(IFileService fileService, IUnitOfWork unitOfWork) : IBrandService
    {
        private readonly IFileService _fileService = fileService;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<Result> AddBrandAsync(BrandRequest request, CancellationToken cancellationToken)
        {
            var brandRepository = _unitOfWork.GetRepository<Brand, int>();
            
            var nameSpec = new BrandSpecification(request.Name);
            var nameIsExist = await brandRepository.GetWithSpecAsync(nameSpec);

            if(nameIsExist != null)
                return Result.Failure(BrandErrors.DublicatedName);

            var uploadResult = await _fileService.UploadToCloudinaryAsync(request.Image!);
            if (!uploadResult.IsSuccess)
                return Result.Failure(FileErrors.UploadFailed);

            var createdBrand = new Brand
            {
                Name = request.Name,
                ImageUrl = uploadResult.Value.SecureUrl,
            };

            var brand = createdBrand.Adapt<Brand>();    
            await brandRepository.AddAsync(brand, cancellationToken);

            await _unitOfWork.CompleteAsync();

            return Result.Success(brand);
        }

        public async Task<Result<IEnumerable<BrandResponse>>> GetAllBrandAsync(CancellationToken cancellationToken)
        {
            var brandRepository = _unitOfWork.GetRepository<Brand, int>();
            var activeBrandsSpec = new BrandSpecification(true);
            var brands = await brandRepository.GetAllWithSpecAsync(activeBrandsSpec);

            return Result.Success(brands.Adapt<IEnumerable<BrandResponse>>());
        }
        public async Task<Result<BrandResponse>> GetBrandByIdAsync(int id, CancellationToken cancellationToken)
        {
            var brandRepository = _unitOfWork.GetRepository<Brand, int>();
            var brandSpec = new BrandSpecification(id);
            var brand = await brandRepository.GetWithSpecAsync(brandSpec);

            if (brand is null || brand.IsDisable)
                return Result.Failure<BrandResponse>(BrandErrors.BrandNotFound);

            return Result.Success(brand.Adapt<BrandResponse>());
        }
        public async Task<Result> UpdateAsync(int id, BrandRequest request, CancellationToken cancellationToken)
        {
            var brandRepository = _unitOfWork.GetRepository<Brand, int>();
            
            var brandSpec = new BrandSpecification(id);
            var brand = await brandRepository.GetWithSpecAsync(brandSpec);

            if (brand is null)
                return Result.Failure(BrandErrors.BrandNotFound);

            var nameSpec = new BrandSpecification(request.Name);
            var nameIsExist = await brandRepository.GetWithSpecAsync(nameSpec);

            if (nameIsExist != null && nameIsExist.Id != id)
            {
                return Result.Failure(BrandErrors.DublicatedName);
            }
            
            if (!string.IsNullOrEmpty(brand.ImageUrl))
            {
                var publicId = _fileService.ExtractPublicIdFromUrl(brand.ImageUrl);
                if (!string.IsNullOrEmpty(publicId))
                    await _fileService.DeleteFromCloudinaryAsync(publicId);
            }
            
            brand.Name = request.Name;
            var uploadResult = await _fileService.UploadToCloudinaryAsync(request.Image!);
            if (!uploadResult.IsSuccess)
                return Result.Failure(FileErrors.UploadFailed);

            brand.ImageUrl = uploadResult.Value.SecureUrl;

            brandRepository.Update(brand);
            await _unitOfWork.CompleteAsync();

            return Result.Success();
        }
        public async Task<Result> DeleteBrandAsync(int id , CancellationToken cancellationToken)
        {
            var brandRepository = _unitOfWork.GetRepository<Brand, int>();
            var brandSpec = new BrandSpecification(id);
            var brand = await brandRepository.GetWithSpecAsync(brandSpec);

            if (brand is null)
                return Result.Failure(BrandErrors.BrandNotFound);

            brand.IsDisable = !brand.IsDisable;
            brandRepository.Update(brand);
            await _unitOfWork.CompleteAsync();

            return Result.Success();
        }
    }
}
