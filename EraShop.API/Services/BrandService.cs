using EraShop.API.Abstractions;
using EraShop.API.Abstractions.Consts;
using EraShop.API.Contracts.Brands;
using EraShop.API.Entities;
using EraShop.API.Errors;
using EraShop.API.Persistence;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System;
using static Org.BouncyCastle.Asn1.Cmp.Challenge;

namespace EraShop.API.Services
{
    public class BrandService(IFileService fileService , ApplicationDbContext context, IHttpContextAccessor httpContextAccessor) : IBrandService
    {
        private readonly IFileService _fileService = fileService;
        private readonly ApplicationDbContext _context = context;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;


        public async Task<Result> AddBrandAsync(BrandRequest request, CancellationToken cancellationToken)
        {
            var nameIsExist = await _context.Brands
                               .AnyAsync(x => x.Name.ToLower() == request.Name.ToLower());

            if(nameIsExist)
            {
                return Result.Failure(BrandErrors.DublicatedName);
            }


            if (request.Image?.Length > 1 * 1024 * 1024)
            {
                return Result.Failure(BrandErrors.BrandImageExcced1M);
            }

            string[] allowedFileExtensions = [".jpg", ".jpeg", ".png"];
            string createdImageName = await _fileService.SaveFileAsync(request.Image!, allowedFileExtensions,"Brand");

            var baseUrl = GetBaseUrl();

            var createdBrand = new Brand
            {
                Name = request.Name,
                ImageUrl = $"{baseUrl}{createdImageName}",
            };

            var brand = createdBrand.Adapt<Brand>();    
            await _context.AddAsync(brand, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success(brand);
        }

        public async Task<Result<IEnumerable<BrandResponse>>> GetAllBrandAsync(CancellationToken cancellationToken)
        {
            var Brands = await _context.Brands.Where(x => !x.IsDisable).AsNoTracking().ToListAsync(cancellationToken);      

            return Result.Success(Brands.Adapt<IEnumerable<BrandResponse>>());
        }

        public async Task<Result<BrandResponse>> GetBrandByIdAsync(int id, CancellationToken cancellationToken)
        {
            var Brand = await _context.Brands.FindAsync(id);

            if (Brand is null || Brand.IsDisable)
                return Result.Failure<BrandResponse>(BrandErrors.BrandNotFound);



            return Result.Success(Brand.Adapt<BrandResponse>());
            
        }
        public async Task<Result> UpdateAsync(int id, BrandRequest request, CancellationToken cancellationToken)
        {
            var Brand = await _context.Brands.FindAsync(id);

            if (Brand is null)
                return Result.Failure(BrandErrors.BrandNotFound);

            var nameIsExist = await _context.Brands
                               .AnyAsync(x => (x.Name.ToLower() == request.Name.ToLower()) && (x.Id != id));

            if (nameIsExist)
            {
                return Result.Failure(BrandErrors.DublicatedName);
            }
            // First Delete Image from folder
            var uri = new Uri(Brand.ImageUrl!);
            string fileName = Path.GetFileName(uri.LocalPath);

            _fileService.DeleteFile(fileName!, "Brand");

            //second Update Name 
            Brand.Name = request.Name;

            // Third Add New Image
            if(request.Image?.Length > 1 * 1024 * 1024)
            {
                return Result.Failure(new Error("invalidFileSize", "File Size shouldn't exceed 1MB", StatusCodes.Status400BadRequest));
            }

            string[] allowedFileExtensions = [".jpg", ".jpeg", ".png"];
            string createdImageName = await _fileService.SaveFileAsync(request.Image!, allowedFileExtensions, "Brand");

            var baseUrl = GetBaseUrl();

            Brand.ImageUrl = $"{baseUrl}{createdImageName}";

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();


        }

        public async Task<Result> DeleteBrandAsync(int id , CancellationToken cancellationToken)
        {
            var Brand = await _context.Brands.FindAsync(id);

            if (Brand is null)
                return Result.Failure(BrandErrors.BrandNotFound);

            Brand.IsDisable = !Brand.IsDisable;

            // This code causes an error when we delete image from the folder if we toggle status again

            //var uri = new Uri(Brand.ImageUrl!);
            //string fileName = Path.GetFileName(uri.LocalPath);

            //_fileService.DeleteFile(fileName!,"Brand");

            await _context.SaveChangesAsync(cancellationToken) ;

            return Result.Success();
        }


        private string GetBaseUrl()
        {
            var request = _httpContextAccessor.HttpContext?.Request;
            if (request == null)
                throw new InvalidOperationException("HttpContext is not available.");

            return $"{request.Scheme}://{request.Host}/Resources/Brand/";
        }


    }
}
