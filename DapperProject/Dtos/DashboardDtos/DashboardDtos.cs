namespace DapperProject.Dtos.DashboardDtos
{
    public class DashboardSummaryDto
    {
        public int TotalOwners { get; set; }
        public int TotalPets { get; set; }
        public int TotalCompleted { get; set; }
        public decimal TotalRevenue { get; set; }
        public int OverdueVaccines { get; set; }
        public int PendingAppointments { get; set; }
    }

    public class MonthlyAppointmentDto
    {
        public string Month { get; set; }
        public int Count { get; set; }
    }

    public class SpeciesDistributionDto
    {
        public string SpeciesName { get; set; }
        public int Count { get; set; }
    }

    public class TopVetDto
    {
        public string FullName { get; set; }
        public int AppointmentCount { get; set; }
    }

    public class CityDistributionDto
    {
        public string City { get; set; }
        public int Count { get; set; }
    }
    public class AppointmentStatusDto
    {
        public string Status { get; set; }
        public int Count { get; set; }
    }

    public class MonthlyRevenueDto
    {
        public string Month { get; set; }
        public decimal Revenue { get; set; }
    }

    public class MonthlyCompletionDto
    {
        public string Month { get; set; }
        public int Total { get; set; }
        public int Completed { get; set; }
        public decimal Rate { get; set; }
    }

    public class RecentAppointmentDto
    {
        public string PetName { get; set; }
        public string OwnerName { get; set; }
        public string City { get; set; }
        public string VetName { get; set; }
        public string AppointmentDate { get; set; }
        public string Status { get; set; }
    }
}
