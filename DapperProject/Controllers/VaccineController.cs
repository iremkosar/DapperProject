using DapperProject.Dtos.VaccineDtos;
using DapperProject.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DapperProject.Controllers
{
    public class VaccineController : Controller
    {
        private readonly IVaccineService _vaccineService;

        public VaccineController(IVaccineService vaccineService)
        {
            _vaccineService = vaccineService;
        }

        public async Task<IActionResult> VaccineList(
            string petName, string vaccineName, string status, int page = 1)
        {
            var (data, totalCount) = await _vaccineService
                .GetFilteredVaccinesAsync(petName, vaccineName, status, page);

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling(totalCount / 12.0);
            ViewBag.TotalCount = totalCount;
            ViewBag.PetName = petName ?? "";
            ViewBag.VaccineName = vaccineName ?? "";
            ViewBag.Status = status ?? "";

            return View(data);
        }

        [HttpGet]
        public IActionResult CreateVaccine()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateVaccine(CreateVaccineDto dto)
        {
            await _vaccineService.CreateVaccineAsync(dto);
            return RedirectToAction("VaccineList");
        }

        public async Task<IActionResult> DeleteVaccine(int id)
        {
            await _vaccineService.DeleteVaccineAsync(id);
            return RedirectToAction("VaccineList");
        }

        [HttpGet]
        public async Task<IActionResult> UpdateVaccine(int id)
        {
            var value = await _vaccineService.GetVaccineByIdAsync(id);
            return View(value);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateVaccine(UpdateVaccineDto dto)
        {
            await _vaccineService.UpdateVaccineAsync(dto);
            return RedirectToAction("VaccineList");
        }
    }
}