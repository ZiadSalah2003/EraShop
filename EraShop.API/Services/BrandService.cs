using EraShop.API.Abstractions;
using EraShop.API.Abstractions.Consts;
using EraShop.API.Contracts.Brands;
using EraShop.API.Contracts.Infrastructure;
using EraShop.API.Entities;
using EraShop.API.Errors;
using EraShop.API.Specification.Brand;
using Mapster;
using System;
using static Org.BouncyCastle.Asn1.Cmp.Challenge;

namespace EraShop.API.Services
{
    public class BrandService(IFileService fileService, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor) : IBrandService
    {
        private readonly IFileService _fileService = fileService;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;


        public async Task<Result> AddBrandAsync(BrandRequest request, CancellationToken cancellationToken)
        {
            var brandRepository = _unitOfWork.GetRepository<Brand, int>();
            
            var nameSpec = new BrandSpecification(request.Name);
            var nameIsExist = await brandRepository.GetWithSpecAsync(nameSpec);

            if(nameIsExist != null)
            {
                return Result.Failure(BrandErrors.DublicatedName);
            }

            string createdImageName = await _fileService.SaveFileAsync(request.Image!,"Brand");

            var baseUrl = GetBaseUrl();

            var createdBrand = new Brand
            {
                Name = request.Name,
                ImageUrl = $"{baseUrl}{createdImageName}",
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
            var uri = new Uri(brand.ImageUrl!);
            string fileName = Path.GetFileName(uri.LocalPath);

            _fileService.DeleteFile(fileName!, "Brand");
            brand.Name = request.Name;

            string createdImageName = await _fileService.SaveFileAsync(request.Image!, "Brand");

            var baseUrl = GetBaseUrl();

            brand.ImageUrl = $"{baseUrl}{createdImageName}";

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

            // This code causes an error when we delete image from the folder if we toggle status again

            //var uri = new Uri(brand.ImageUrl!);
            //string fileName = Path.GetFileName(uri.LocalPath);

            //_fileService.DeleteFile(fileName!,"Brand");

            brandRepository.Update(brand);
            await _unitOfWork.CompleteAsync();

            return Result.Success();
        }
        private string GetBaseUrl()
        {
            var request = _httpContextAccessor.HttpContext?.Request;
            if (request == null)
                throw new InvalidOperationException("HttpContext is not available.");

			return $"{request.Scheme}://{request.Host}/images/Brand/";
		}


    }
}
