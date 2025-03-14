namespace EraShop.API.Contracts.Report
{

    public record SoldProductDto(
        int ProductId,
        string ProductName,
        string ProductImage,
        int SoldQuantity
    );
}
