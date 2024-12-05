using EraShop.API.Abstractions;
using EraShop.API.Contracts.Brands;

namespace EraShop.API.Services
{
    public interface IBrandService
    {
        Task<Result> AddBrandAsync(BrandRequest request , CancellationToken cancellationToken);
        Task<Result<IEnumerable<BrandResponse>>> GetAllBrandAsync(CancellationToken cancellationToken);

    }
}
