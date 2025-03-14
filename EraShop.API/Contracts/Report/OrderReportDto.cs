namespace EraShop.API.Contracts.Report
{
    public record OrderReportDto(
        int OrderId,
        string UserEmail,
        string OrderStatus,
        string UserAddress,
        string DeliveryMethod
    );
}
