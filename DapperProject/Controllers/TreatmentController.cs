using DapperProject.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DapperProject.Controllers
{
    public class TreatmentController : Controller
    {
        private readonly ITreatmentService _treatmentService;

        public TreatmentController(ITreatmentService treatmentService)
        {
            _treatmentService = treatmentService;
        }

        public async Task<IActionResult> TreatmentList(
            string petName, string diagnosis, string vetName, int page = 1)
        {
            var (data, totalCount) = await _treatmentService
                .GetFilteredTreatmentsAsync(petName, diagnosis, vetName, page);

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling(totalCount / 12.0);
            ViewBag.TotalCount = totalCount;
            ViewBag.PetName = petName ?? "";
            ViewBag.Diagnosis = diagnosis ?? "";
            ViewBag.VetName = vetName ?? "";

            return View(data);
        }

        public async Task<IActionResult> TreatmentDetail(int id)
        {
            var treatment = await _treatmentService.GetTreatmentByIdAsync(id);
            var prescriptions = await _treatmentService.GetPrescriptionsByTreatmentIdAsync(id);

            ViewBag.Prescriptions = prescriptions;
            return View(treatment);
        }
    }
}