using DapperProject.Dtos.TreatmentDtos;

namespace DapperProject.Repositories
{
    public interface ITreatmentService
    {
        Task<(List<ResultTreatmentDto> Data, int TotalCount)> GetFilteredTreatmentsAsync(
            string petName, string diagnosis, string vetName, int page, int pageSize = 12);
        Task<GetTreatmentByIdDto> GetTreatmentByIdAsync(int id);
        Task<List<ResultPrescriptionDto>> GetPrescriptionsByTreatmentIdAsync(int treatmentId);
    }
}