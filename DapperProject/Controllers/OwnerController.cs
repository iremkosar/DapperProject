using DapperProject.Dtos.OwnerDtos;
using DapperProject.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DapperProject.Controllers
{
    public class OwnerController : Controller
    {
        private readonly IOwnerService _ownerService;
        private readonly ExcelService _excelService;

        public OwnerController(IOwnerService ownerService, ExcelService excelService)
        {
            _ownerService = ownerService;
            _excelService = excelService;
        }
        public async Task<IActionResult> OwnerList(string fullName, string city, string gender, int page = 1)
        {
            var allValues = await _ownerService.GetAllOwnersAsync();

            // Backend filtreleme
            if (!string.IsNullOrEmpty(fullName))
                allValues = allValues.Where(o => o.FullName.Contains(fullName, StringComparison.OrdinalIgnoreCase)).ToList();
            if (!string.IsNullOrEmpty(city))
                allValues = allValues.Where(o => o.City == city).ToList();
            if (!string.IsNullOrEmpty(gender))
                allValues = allValues.Where(o => o.Gender?.Trim() == gender).ToList();

            int pageSize = 12;
            int total = allValues.Count;
            var paged = allValues.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling(total / (double)pageSize);
            ViewBag.TotalCount = total;
            ViewBag.FullName = fullName ?? "";
            ViewBag.City = city ?? "";
            ViewBag.Gender = gender ?? "";

            return View(paged);
        }
        [HttpGet]
        public IActionResult CreateOwner()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateOwner(CreateOwnerDto dto)
        {
            await _ownerService.CreateOwnerAsync(dto);
            return RedirectToAction("OwnerList");
        }
        public async Task<IActionResult> DeleteOwner(int id)
        {
            await _ownerService.GetOwnerByIdAsync(id);
            return RedirectToAction("OwnerList");
        }
        [HttpGet]
        public async Task<IActionResult> UpdateOwner(int id)
        {
            var value = await _ownerService.GetOwnerByIdAsync(id);
            return View(value);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateOwner(UpdateOwnerDto dto)
        {
            await _ownerService.UpdateOwnerAsync(dto);
            return RedirectToAction("OwnerList");
        }
        public async Task<IActionResult> OwnerDetail(int id)
        {
            var owner = await _ownerService.GetOwnerDetailAsync(id);
            var pets = await _ownerService.GetOwnerPetsAsync(id);
            var appointments = await _ownerService.GetOwnerAppointmentsAsync(id);

            ViewBag.Pets = pets;
            ViewBag.Appointments = appointments;

            return View(owner);
        }
        public async Task<IActionResult> ExportExcel()
        {
            var data = await _ownerService.GetAllOwnersAsync();
            var bytes = _excelService.ExportOwners(data);
            return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"Sahipler_{DateTime.Now:yyyyMMdd}.xlsx");
        }
    }
}
