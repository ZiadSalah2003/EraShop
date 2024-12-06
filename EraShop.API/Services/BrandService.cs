using EraShop.API.Abstractions;
using EraShop.API.Abstractions.Consts;
using EraShop.API.Contracts.Brands;
using EraShop.API.Entities;
using EraShop.API.Errors;
using EraShop.API.Persistence;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace EraShop.API.Services
{
    public class BrandService(IFileService fileService , ApplicationDbContext context) : IBrandService
    {
        private readonly IFileService _fileService = fileService;
        private readonly ApplicationDbContext _context = context;

        public async Task<Result> AddBrandAsync(BrandRequest request, CancellationToken cancellationToken)
        {
            if (request.Image?.Length > 1 * 1024 * 1024)
            {
                return Result.Failure(BrandErrors.BrandImageExcced1M);
            }

            string[] allowedFileExtensions = [".jpg", ".jpeg", ".png"];
            string createdImageName = await _fileService.SaveFileAsync(request.Image!, allowedFileExtensions, ImageSubFolder.Brand);

            var createdBrand = new Brand
            {
                Name = request.Name,
                ImageUrl = createdImageName,
            };

            var brand = createdBrand.Adapt<Brand>();    
            await _context.AddAsync(brand, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success(brand);
        }

        public async Task<Result<IEnumerable<BrandResponse>>> GetAllBrandAsync(CancellationToken cancellationToken)
        {
            var Brands = await _context.Brands.AsNoTracking().ToListAsync(cancellationToken);      

            return Result.Success(Brands.Adapt<IEnumerable<BrandResponse>>());
        }
    }
}
