
using EraShop.API.Contracts.WishList;

namespace EraShop.API.Services
{
    public interface IWishListService
    {
        Task<Result> AddListAsync(CreateListRequest request, CancellationToken cancellationToken);
        Task<Result> AddProductToWishListAsync(int id , AddProudctToListRequest request, CancellationToken cancellationToken);
        Task<Result<WishListResponse>> GetProductsFromWishListAsync(int id,  CancellationToken cancellationToken);
        Task<Result<IEnumerable<GetAllWishListsResponse>>> GetAllWishListsAsync(CancellationToken cancellationToken);
        Task<Result> UpdateWishListAsync(int id,UpdateWishListRequest request , CancellationToken cancellationToken);
        Task<Result> DeleteProductFromWishListAsync(int id, DeleteProductFromListRequest request, CancellationToken cancellationToken);
        Task<Result> DeleteWishListAsync(int id, CancellationToken cancellationToken);
    }
}
