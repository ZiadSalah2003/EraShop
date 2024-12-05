namespace EraShop.API.Contracts.Brands
{
    public record BrandRequest
    (
        string Name,
        IFormFile? Image
    );

    
}
