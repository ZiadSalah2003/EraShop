using EraShop.API.Contracts.Products;

namespace EraShop.API.Contracts.WishList
{
    public record WishListResponse
    (
        int id,
        IEnumerable<WishListProductResponse> Products
    );
}
