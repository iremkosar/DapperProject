using DapperProject.Dtos.PetDtos;
using DapperProject.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DapperProject.Controllers
{
    public class PetController : Controller
    {
        private readonly IPetService _petService;
        private readonly ExcelService _excelService;

        public PetController(IPetService petService, ExcelService excelService)
        {
            _petService = petService;
            _excelService = excelService;
        }

        public async Task<IActionResult> PetList(string name, string species, string gender, int page = 1)
        {
            var allValues = await _petService.GetAllPetsAsync();

            if (!string.IsNullOrEmpty(name))
                allValues = allValues.Where(p => p.Name.Contains(name, StringComparison.OrdinalIgnoreCase)).ToList();
            if (!string.IsNullOrEmpty(species))
                allValues = allValues.Where(p => p.SpeciesName == species).ToList();
            if (!string.IsNullOrEmpty(gender))
                allValues = allValues.Where(p => p.Gender == gender).ToList();

            int pageSize = 12;
            int total = allValues.Count;
            var paged = allValues.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling(total / (double)pageSize);
            ViewBag.TotalCount = total;
            ViewBag.Name = name ?? "";
            ViewBag.Species = species ?? "";
            ViewBag.Gender = gender ?? "";

            return View(paged);
        }

        [HttpGet]
        public IActionResult CreatePet()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreatePet(CreatePetDto dto)
        {
            await _petService.CreatePetAsync(dto);
            return RedirectToAction("PetList");
        }

        public async Task<IActionResult> DeletePet(int id)
        {
            await _petService.DeletePetAsync(id);
            return RedirectToAction("PetList");
        }

        [HttpGet]
        public async Task<IActionResult> UpdatePet(int id)
        {
            var value = await _petService.GetPetByIdAsync(id);
            return View(value);
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePet(UpdatePetDto dto)
        {
            await _petService.UpdatePetAsync(dto);
            return RedirectToAction("PetList");
        }
        public async Task<IActionResult> PetDetail(int id)
        {
            var pet = await _petService.GetPetDetailAsync(id);
            var appointments = await _petService.GetPetAppointmentsAsync(id);
            var vaccines = await _petService.GetPetVaccinesAsync(id);

            ViewBag.Appointments = appointments;
            ViewBag.Vaccines = vaccines;

            return View(pet);
        }
        public async Task<IActionResult> ExportExcel()
        {
            var data = await _petService.GetAllPetsAsync();
            var bytes = _excelService.ExportPets(data);
            return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"Hayvanlar_{DateTime.Now:yyyyMMdd}.xlsx");
        }
    }
}