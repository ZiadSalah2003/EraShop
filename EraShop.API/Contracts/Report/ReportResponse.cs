namespace EraShop.API.Contracts.Report
{
    public record ReportResponse(
        int NewUsersCount,
        int OrdersCount,
        decimal TotalRevenue,
        int TotalSoldProducts,
        int TotalSoldPieces,
        List<OrderReportDto> Orders,
        List<SoldProductDto> SoldProducts
    );
}


