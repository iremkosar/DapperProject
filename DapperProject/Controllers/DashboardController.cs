using DapperProject.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DapperProject.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        public async Task<IActionResult> Index()
        {
            var summary = await _dashboardService.GetSummaryAsync();
            var monthly = await _dashboardService.GetMonthlyAppointmentsAsync();
            var species = await _dashboardService.GetSpeciesDistributionAsync();
            var topVets = await _dashboardService.GetTopVetsAsync();
            var cities = await _dashboardService.GetCityDistributionAsync();
            var statusDist = await _dashboardService.GetAppointmentStatusDistributionAsync();
            var revenue = await _dashboardService.GetMonthlyRevenueAsync();
            var completion = await _dashboardService.GetMonthlyCompletionRateAsync();
            var recent = await _dashboardService.GetRecentAppointmentsAsync();

            ViewBag.Summary = summary;
            ViewBag.Monthly = monthly;
            ViewBag.Species = species;
            ViewBag.TopVets = topVets;
            ViewBag.Cities = cities;
            ViewBag.StatusDist = statusDist;
            ViewBag.Revenue = revenue;
            ViewBag.Completion = completion;
            ViewBag.Recent = recent;

            return View();
        }
    }
}
