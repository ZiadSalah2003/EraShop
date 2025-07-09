using EraShop.API.Contracts.Infrastructure;
using EraShop.API.Contracts.Products;
using EraShop.API.Contracts.WishList;
using EraShop.API.Entities;
using EraShop.API.Errors;
using EraShop.API.Specification.Product;
using EraShop.API.Specification.WishList;
using Mapster;
using System.Security.Claims;

namespace EraShop.API.Services
{
    public class WishListService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor) : IWishListService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public async Task<Result> AddListAsync(CreateListRequest request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var listRepository = _unitOfWork.GetRepository<List, int>();
            
            var nameSpec = new WishListSpecification(l => l.Name.ToLower() == request.Name.ToLower() && l.UserId == userId);
            var nameIsExist = await listRepository.GetWithSpecAsync(nameSpec);

            if (nameIsExist != null)
                return Result.Failure(WishListErrors.ListNameIsExist);

            var newList = new List
            {
                UserId = userId!,
                Name = request.Name,
            };

            var list = newList.Adapt<List>();

            await listRepository.AddAsync(list, cancellationToken);
            await _unitOfWork.CompleteAsync();
            return Result.Success();
        }

        public async Task<Result> AddProductToWishListAsync(int id, AddProudctToListRequest request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var listRepository = _unitOfWork.GetRepository<List, int>();
            var productRepository = _unitOfWork.GetRepository<Product, int>();
            var listItemRepository = _unitOfWork.GetRepository<ListItem, int>();
            
            var wishListSpec = new WishListSpecification(l => l.Id == id && l.UserId == userId);
            var wishListExist = await listRepository.GetWithSpecAsync(wishListSpec);

            if(wishListExist == null)
                return Result.Failure(WishListErrors.ListNotFound);

            var productSpec = new ProductSpecification(request.ProductId);
            var productIsExist = await productRepository.GetWithSpecAsync(productSpec);

            if (productIsExist == null)
                return Result.Failure(ProductErrors.ProductNotFound);

            var listItemSpec = new ListItemSpecification(id, request.ProductId);
            var productIsExistInList = await listItemRepository.GetWithSpecAsync(listItemSpec);
           
            if (productIsExistInList != null)
                return Result.Failure(WishListErrors.ProductAlreadyExistInList);

            var addingProductToList = new ListItem 
            { 
                ProductId = request.ProductId,
                ListId = id 
            };

            var addedProduct = addingProductToList.Adapt<ListItem>();

            await listItemRepository.AddAsync(addedProduct, cancellationToken);
            await _unitOfWork.CompleteAsync();
            return Result.Success();
        }


        public async Task<Result<WishListResponse>> GetProductsFromWishListAsync(int id, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var listRepository = _unitOfWork.GetRepository<List, int>();
            var wishListSpec = new WishListSpecification(l => l.Id == id && l.UserId == userId);
            var wishList = await listRepository.GetWithSpecAsync(wishListSpec);

            if (wishList == null)
                return Result.Failure<WishListResponse>(WishListErrors.ListNotFound);

            var productRepository = _unitOfWork.GetRepository<Product, int>();
            var productIds = wishList.Items.Select(i => i.ProductId).ToList();
            var products = new List<Product>();

            foreach (var productId in productIds)
            {
                var productSpec = new ProductSpecification(productId);
                var product = await productRepository.GetWithSpecAsync(productSpec);
                if (product != null)
                    products.Add(product);
            }
            var wishListResponse = new WishListResponse(
                wishList.Id,
                products.Select(product => new WishListProductResponse(
                    product.Id,
                    product.Name,
                    product.Description,
                    product.Price,
                    product.ImageUrl!,
                    product.Quantity,
                    product.IsDisable,
                    product.Brand?.Name ?? "Unknown",
                    product.Category?.Name ?? "Unknown"
                ))
            );

            return Result.Success(wishListResponse);
        }
        public async Task<Result<IEnumerable<GetAllWishListsResponse>>> GetAllWishListsAsync(CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
                return Result.Failure<IEnumerable<GetAllWishListsResponse>>(UserErrors.UserEmailNotFound);

            var listRepository = _unitOfWork.GetRepository<List, int>();
            var wishListSpec = new WishListSpecification(userId);
            var wishLists = await listRepository.GetAllWithSpecAsync(wishListSpec);

            var wishListResponses = wishLists.Select(l => new GetAllWishListsResponse(l.Id, l.Name));

            return Result.Success<IEnumerable<GetAllWishListsResponse>>(wishListResponses);
        }
        public async Task<Result> UpdateWishListAsync(int id, UpdateWishListRequest request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var listRepository = _unitOfWork.GetRepository<List, int>();
            var wishListSpec = new WishListSpecification(l => l.Id == id && l.UserId == userId);
            var wishList = await listRepository.GetWithSpecAsync(wishListSpec);

            if (wishList == null)
                return Result.Failure(WishListErrors.ListNotFound);

            var nameSpec = new WishListSpecification(l => l.UserId == userId && l.Name.ToLower() == request.Name.ToLower() && l.Id != id);
            var isNameTaken = await listRepository.GetWithSpecAsync(nameSpec);

            if (isNameTaken != null)
                return Result.Failure(WishListErrors.ListNameIsExist);

            wishList.Name = request.Name;
            listRepository.Update(wishList);
            await _unitOfWork.CompleteAsync();

            return Result.Success();
        }
        public async Task<Result> DeleteProductFromWishListAsync(int id, DeleteProductFromListRequest request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var listRepository = _unitOfWork.GetRepository<List, int>();
            var productRepository = _unitOfWork.GetRepository<Product, int>();
            var listItemRepository = _unitOfWork.GetRepository<ListItem, int>();
            
            var wishListSpec = new WishListSpecification(l => l.Id == id && l.UserId == userId);
            var wishListExist = await listRepository.GetWithSpecAsync(wishListSpec);

            if (wishListExist == null)
                return Result.Failure(WishListErrors.ListNotFound);

            var productSpec = new ProductSpecification(request.ProductId);
            var productIsExist = await productRepository.GetWithSpecAsync(productSpec);

            if (productIsExist == null)
                return Result.Failure(ProductErrors.ProductNotFound);

            var listItemSpec = new ListItemSpecification(id, request.ProductId);
            var listItem = await listItemRepository.GetWithSpecAsync(listItemSpec);
            
            if (listItem == null)
                return Result.Failure(ProductErrors.ProductNotFound);

            listItemRepository.Delete(listItem);
            await _unitOfWork.CompleteAsync();

            return Result.Success();
        }
        public async Task<Result> DeleteWishListAsync(int id, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var listRepository = _unitOfWork.GetRepository<List, int>();
            var listItemRepository = _unitOfWork.GetRepository<ListItem, int>();
            
            var wishListSpec = new WishListSpecification(l => l.Id == id && l.UserId == userId);
            var wishList = await listRepository.GetWithSpecAsync(wishListSpec);

            if (wishList == null)
                return Result.Failure(WishListErrors.ListNotFound);

            var listItemSpec = new ListItemSpecification(id);
            var listItems = await listItemRepository.GetAllWithSpecAsync(listItemSpec);
            foreach (var item in listItems)
                listItemRepository.Delete(item);

            listRepository.Delete(wishList);
            await _unitOfWork.CompleteAsync();

            return Result.Success();
        }


    }
}
