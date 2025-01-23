using EraShop.API.Abstractions;
using EraShop.API.Contracts.Brands;

namespace EraShop.API.Services
{
    public interface IBrandService
    {
        Task<Result> AddBrandAsync(BrandRequest request , CancellationToken cancellationToken);
        Task<Result<IEnumerable<BrandResponse>>> GetAllBrandAsync(CancellationToken cancellationToken);
        Task<Result> DeleteBrandAsync(int id, CancellationToken cancellationToken);
        Task<Result<BrandResponse>> GetBrandByIdAsync(int id, CancellationToken cancellationToken);
        Task<Result> UpdateAsync(int id, BrandRequest request, CancellationToken cancellationToken);
    }
}
