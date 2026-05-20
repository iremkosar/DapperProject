using DapperProject.Dtos.AppointmentDtos;
using DapperProject.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DapperProject.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IPetService _petService;
        private readonly IVetService _vetService;
        private readonly ExcelService _excelService;

        public AppointmentController(
            IAppointmentService appointmentService,
            IPetService petService,
            IVetService vetService,
            ExcelService excelService)
        {
            _appointmentService = appointmentService;
            _petService = petService;
            _vetService = vetService;
            _excelService = excelService;
        }
        public async Task<IActionResult> AppointmentList(
           string petName, string status, string city, string vetName, int page = 1)
        {
            var (data, totalCount) = await _appointmentService
                .GetFilteredAppointmentsAsync(petName, status, city, vetName, page);

            // Sayfalama bilgileri
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling(totalCount / 12.0);
            ViewBag.TotalCount = totalCount;

            // Filtre değerlerini View'da korumak için
            ViewBag.PetName = petName;
            ViewBag.Status = status;
            ViewBag.City = city;
            ViewBag.VetName = vetName;

            return View(data);
        }
        [HttpGet]
        public IActionResult CreateAppointment()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateAppointment(CreateAppointmentDto dto)
        {
            await _appointmentService.CreateAppointmentAsync(dto);
            return RedirectToAction("AppointmentList");
        }
        public async Task<IActionResult> DeleteAppointment(int id)
        {
            await _appointmentService.DeleteAppointmentAsync(id);
            return RedirectToAction("AppointmentList");
        }
        [HttpGet]
        public async Task<IActionResult> UpdateAppointment(int id)
        {
            var value= await _appointmentService.GetAppointmentByIdAsync(id);
            return View(value);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateAppointment(UpdateAppointmentDto dto)
        {
            await _appointmentService.UpdateAppointmentAsync(dto);
            return RedirectToAction("AppointmentList");
        }
        public async Task<IActionResult> ExportExcel(
    string petName, string status, string city, string vetName)
        {
            var (data, _) = await _appointmentService
                .GetFilteredAppointmentsAsync(petName, status, city, vetName, 1, int.MaxValue);
            var bytes = _excelService.ExportAppointments(data);
            return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"Randevular_{DateTime.Now:yyyyMMdd}.xlsx");
        }
    }
}
