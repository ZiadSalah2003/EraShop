namespace EraShop.API.Contracts.Brands
{
    public record BrandResponse
    (
     int Id ,
     string Name,   
     bool IsDisable,
     string ImageUrl  
     );
}
