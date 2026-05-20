using DapperProject.Dtos.VetDtos;
using DapperProject.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DapperProject.Controllers
{
    public class VetController : Controller
    {
        private readonly IVetService _vetService;
        private readonly ExcelService _excelService;

        public VetController(IVetService vetService, ExcelService excelService)
        {
            _vetService = vetService;
            _excelService = excelService;
        }

        public async Task<IActionResult> VetList()
        {
            var values= await _vetService.GetAllVetsAsync();
            return View(values);
        }
        [HttpGet]
        public IActionResult CreateVet()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateVet(CreateVetDto dto)
        {
            await _vetService.CreateVetAsync(dto);
            return RedirectToAction("VetList");
           
        }
        public async Task<IActionResult> DeleteVet(int id)
        {
            await _vetService.DeleteVetAsync(id);
            return RedirectToAction("VetList");
        }
        [HttpGet]
        public async Task<IActionResult> UpdateVet(int id)
        {
            var value= await _vetService.GetVetByIdAsync(id);
            return View(value);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateVet(UpdateVetDto dto)
        {
            await _vetService.UpdateVetAsync(dto);
            return RedirectToAction("VetList");
        }
        public async Task<IActionResult> ExportExcel()
        {
            var data = await _vetService.GetAllVetsAsync();
            var bytes = _excelService.ExportVets(data);
            return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"Veterinerler_{DateTime.Now:yyyyMMdd}.xlsx");
        }

    }
}
