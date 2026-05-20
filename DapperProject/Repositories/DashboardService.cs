using Dapper;
using DapperProject.Context;
using DapperProject.Dtos.DashboardDtos;

namespace DapperProject.Repositories
{
    public class DashboardService:IDashboardService
    {
        private readonly DapperContext _context;

        public DashboardService(DapperContext context)
        {
            _context = context;
        }


        public async Task<DashboardSummaryDto> GetSummaryAsync()
        {
            string query = @"
    SELECT
        (SELECT COUNT(*) FROM Owners) AS TotalOwners,
        (SELECT COUNT(*) FROM Pets) AS TotalPets,
        (SELECT COUNT(*) FROM Appointments WHERE Status='Tamamlandı') AS TotalCompleted,
        (SELECT ISNULL(SUM(TotalCost),0) FROM Treatments) AS TotalRevenue,
        (SELECT COUNT(*) FROM Vaccines 
         WHERE TRY_CONVERT(DATE, NextDueDate, 23) <= CAST(GETDATE() AS DATE)) AS OverdueVaccines,
        (SELECT COUNT(*) FROM Appointments 
         WHERE Status='Bekliyor') AS PendingAppointments";

            var connection = _context.CreateConnection();
            return await connection.QueryFirstAsync<DashboardSummaryDto>(query);
        }


        public async Task<List<MonthlyAppointmentDto>> GetMonthlyAppointmentsAsync()
        {
            string query = @"
        SELECT TOP 24
            FORMAT(CONVERT(DATETIME, AppointmentDate, 120), 'yyyy-MM') AS Month,
            COUNT(*) AS Count
        FROM Appointments
        WHERE TRY_CONVERT(DATETIME, AppointmentDate, 120) IS NOT NULL
        GROUP BY FORMAT(CONVERT(DATETIME, AppointmentDate, 120), 'yyyy-MM')
        ORDER BY Month DESC";

            var connection = _context.CreateConnection();
            var values = await connection.QueryAsync<MonthlyAppointmentDto>(query);
            return values.OrderBy(x => x.Month).ToList();
        }


        public async Task<List<SpeciesDistributionDto>> GetSpeciesDistributionAsync()
        {
            string query = @"
                SELECT s.SpeciesName, COUNT(p.PetId) AS Count
                FROM Pets p
                JOIN Species s ON p.SpeciesId = s.SpeciesId
                GROUP BY s.SpeciesName
                ORDER BY Count DESC";

            var connection = _context.CreateConnection();
            var values = await connection.QueryAsync<SpeciesDistributionDto>(query);
            return values.ToList();
        }

       
        public async Task<List<TopVetDto>> GetTopVetsAsync()
        {
            string query = @"
                SELECT TOP 5 v.FullName, COUNT(a.AppointmentId) AS AppointmentCount
                FROM Appointments a
                JOIN Vets v ON a.VetId = v.VetId
                GROUP BY v.FullName
                ORDER BY AppointmentCount DESC";

            var connection = _context.CreateConnection();
            var values = await connection.QueryAsync<TopVetDto>(query);
            return values.ToList();
        }

       
        public async Task<List<CityDistributionDto>> GetCityDistributionAsync()
        {
            string query = @"
                SELECT City, COUNT(*) AS Count
                FROM Owners
                GROUP BY City
                ORDER BY Count DESC";

            var connection = _context.CreateConnection();
            var values = await connection.QueryAsync<CityDistributionDto>(query);
            return values.ToList();
        }
        // Randevu durumu dağılımı - Pie Chart için
        public async Task<List<AppointmentStatusDto>> GetAppointmentStatusDistributionAsync()
        {
            string query = @"
        SELECT Status, COUNT(*) AS Count
        FROM Appointments
        GROUP BY Status";

            var connection = _context.CreateConnection();
            var values = await connection.QueryAsync<AppointmentStatusDto>(query);
            return values.ToList();
        }

        // Aylık gelir - Bar Chart için
        public async Task<List<MonthlyRevenueDto>> GetMonthlyRevenueAsync()
        {
            string query = @"
        SELECT TOP 24
            FORMAT(CONVERT(DATETIME, TreatmentDate, 120), 'yyyy-MM') AS Month,
            SUM(TotalCost) AS Revenue
        FROM Treatments
        WHERE TRY_CONVERT(DATETIME, TreatmentDate, 120) IS NOT NULL
        GROUP BY FORMAT(CONVERT(DATETIME, TreatmentDate, 120), 'yyyy-MM')
        ORDER BY Month DESC";

            var connection = _context.CreateConnection();
            var values = await connection.QueryAsync<MonthlyRevenueDto>(query);
            return values.OrderBy(x => x.Month).ToList();
        }

        // Tamamlanma oranı - Line Chart için
        public async Task<List<MonthlyCompletionDto>> GetMonthlyCompletionRateAsync()
        {
            string query = @"
        SELECT 
            FORMAT(CONVERT(DATETIME, AppointmentDate, 120), 'yyyy-MM') AS Month,
            COUNT(*) AS Total,
            SUM(CASE WHEN Status = 'Tamamlandı' THEN 1 ELSE 0 END) AS Completed,
            CAST(SUM(CASE WHEN Status = 'Tamamlandı' THEN 1 ELSE 0 END) * 100.0 / COUNT(*) AS DECIMAL(5,1)) AS Rate
        FROM Appointments
        WHERE TRY_CONVERT(DATETIME, AppointmentDate, 120) IS NOT NULL
        GROUP BY FORMAT(CONVERT(DATETIME, AppointmentDate, 120), 'yyyy-MM')
        ORDER BY Month";

            var connection = _context.CreateConnection();
            var values = await connection.QueryAsync<MonthlyCompletionDto>(query);
            return values.ToList();
        }

        // Son 5 randevu - Tablo için
        public async Task<List<RecentAppointmentDto>> GetRecentAppointmentsAsync()
        {
            string query = @"
        SELECT TOP 5
            p.Name AS PetName,
            o.FullName AS OwnerName,
            o.City,
            v.FullName AS VetName,
            a.AppointmentDate,
            a.Status
        FROM Appointments a
        JOIN Pets p    ON a.PetId = p.PetId
        JOIN Owners o  ON p.OwnerId = o.OwnerId
        JOIN Vets v    ON a.VetId = v.VetId
        ORDER BY TRY_CONVERT(DATETIME, a.AppointmentDate, 120) DESC";

            var connection = _context.CreateConnection();
            var values = await connection.QueryAsync<RecentAppointmentDto>(query);
            return values.ToList();
        }
    }
}
