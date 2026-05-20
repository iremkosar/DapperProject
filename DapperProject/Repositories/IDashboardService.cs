using DapperProject.Dtos.DashboardDtos;

namespace DapperProject.Repositories
{
    public interface IDashboardService
    {
        Task<DashboardSummaryDto> GetSummaryAsync();
        Task<List<MonthlyAppointmentDto>> GetMonthlyAppointmentsAsync();
        Task<List<SpeciesDistributionDto>> GetSpeciesDistributionAsync();
        Task<List<TopVetDto>> GetTopVetsAsync();
        Task<List<CityDistributionDto>> GetCityDistributionAsync();
        Task<List<AppointmentStatusDto>> GetAppointmentStatusDistributionAsync();
        Task<List<MonthlyRevenueDto>> GetMonthlyRevenueAsync();
        Task<List<MonthlyCompletionDto>> GetMonthlyCompletionRateAsync();
        Task<List<RecentAppointmentDto>> GetRecentAppointmentsAsync();
    }
}
