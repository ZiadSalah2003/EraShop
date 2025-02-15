using EraShop.API.Contracts.Products;
using EraShop.API.Contracts.WishList;
using EraShop.API.Entities;
using EraShop.API.Errors;
using EraShop.API.Persistence;
using Mapster;
using System.Collections.Generic;
using System.Security.Claims;

namespace EraShop.API.Services
{
    public class WishListService(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor) : IWishListService
    {
        private readonly ApplicationDbContext _context = context;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public async Task<Result> AddListAsync(CreateListRequest request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var NameIsExist = await _context.Lists
                .AnyAsync(x => x.Name.ToLower() == request.Name.ToLower() && x.UserId == userId , cancellationToken);

            if (NameIsExist)
                return Result.Failure(WishListErrors.ListNameIsExist);

            

            var newList = new List
            {
                UserId = userId!,
                Name = request.Name,
            };

            var list = newList.Adapt<List>();

            await _context.Lists.AddAsync(list, cancellationToken);
            await _context.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<Result> AddProductToWishListAsync(int id, AddProudctToListRequest request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var WishListExist = await _context.Lists.AnyAsync(l => l.Id == id && l.UserId == userId , cancellationToken) ; 

            if(!WishListExist)
                return Result.Failure(WishListErrors.ListNotFound);

            var ProductIsExist = await _context.Products.AnyAsync(p => p.Id == request.ProductId, cancellationToken);

            if (!ProductIsExist)
                return Result.Failure(ProductErrors.ProductNotFound);

            var ProductIsExistInList = await _context.ListItems.AnyAsync(l => l.ListId == id && l.ProductId == request.ProductId , cancellationToken);
           
            if (ProductIsExistInList)
                return Result.Failure(WishListErrors.ProductAlreadyExistInList);

            var addingProductToList = new ListItem 
            { 
                ProductId = request.ProductId,
                ListId = id 
            };

            var addedProduct = addingProductToList.Adapt<ListItem>();

            await _context.ListItems.AddAsync(addedProduct, cancellationToken);
            await _context.SaveChangesAsync();
            return Result.Success();
        }


        public async Task<Result<WishListResponse>> GetProductsFromWishListAsync(int id, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var WishListExist = await _context.Lists.AnyAsync(l => l.Id == id && l.UserId == userId, cancellationToken);

            if (!WishListExist)
                return Result.Failure<WishListResponse>(WishListErrors.ListNotFound);

                 var wishList = await _context.Lists
                     .Where(l => l.Id == id && l.UserId == userId)
                     .Select(l => new WishListResponse(
                         l.Id,
                         l.Items.Select(i => new WishListProductResponse(
                             i.Product.Id,
                             i.Product.Name,
                             i.Product.Description,
                             i.Product.Price,
                             i.Product.ImageUrl!,
                             i.Product.Quantity,
                             i.Product.IsDisable,
                             i.Product.Brand!.Name,
                             i.Product.Category!.Name
                         ))
                      ))
                 .FirstOrDefaultAsync(cancellationToken);

            return Result.Success(wishList!);


        }

        public async Task<Result<IEnumerable<GetAllWishListsResponse>>> GetAllWishListsAsync(CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var wishList = await _context.Lists
                                    .Where(l => l.UserId == userId) 
                                    .Select(l => new GetAllWishListsResponse(l.Id, l.Name)) 
                                    .ToListAsync(cancellationToken);

            return Result.Success<IEnumerable<GetAllWishListsResponse>>(wishList);
        }

        public async Task<Result> UpdateWishListAsync(int id, UpdateWishListRequest request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var wishList = await _context.Lists
                            .FirstOrDefaultAsync(l => l.Id == id && l.UserId == userId, cancellationToken);

            if (wishList is null)
                return Result.Failure(WishListErrors.ListNotFound);

            var isNameTaken = await _context.Lists
                         .AnyAsync(l => l.UserId == userId && l.Name.ToLower() == request.Name.ToLower() && l.Id != id, cancellationToken);

            if (isNameTaken)
                return Result.Failure(WishListErrors.ListNameIsExist);

            wishList.Name = request.Name;
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();

        }

        public async Task<Result> DeleteProductFromWishListAsync(int id, DeleteProductFromListRequest request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var WishListExist = await _context.Lists.AnyAsync(l => l.Id == id && l.UserId == userId, cancellationToken);

            if (!WishListExist)
                return Result.Failure(WishListErrors.ListNotFound);

            var ProductIsExist = await _context.Products.AnyAsync(p => p.Id == request.ProductId, cancellationToken);

            if (!ProductIsExist)
                return Result.Failure(ProductErrors.ProductNotFound);

            var listItem = await _context.ListItems
                             .FirstOrDefaultAsync(l => l.ListId == id && l.ProductId == request.ProductId, cancellationToken);

            if (listItem is null)
                return Result.Failure(ProductErrors.ProductNotFound);

            _context.ListItems.Remove(listItem);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }

        public async Task<Result> DeleteWishListAsync(int id, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var wishList = await _context.Lists
             .Include(l => l.Items) 
             .FirstOrDefaultAsync(l => l.Id == id && l.UserId == userId, cancellationToken);

            if (wishList is null)
                return Result.Failure(WishListErrors.ListNotFound);

            _context.ListItems.RemoveRange(wishList.Items); 
            _context.Lists.Remove(wishList); 

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }


    }
}
