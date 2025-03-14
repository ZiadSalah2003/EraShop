using EraShop.API.Contracts.Report;

namespace EraShop.API.Services
{
    public interface IReportService
    {
        Task<Result<ReportResponse>> GetDailyReport();
    }
}
