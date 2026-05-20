using DapperProject.Dtos.VaccineDtos;

namespace DapperProject.Repositories
{
    public interface IVaccineService
    {
        Task<(List<ResultVaccineDto> Data, int TotalCount)> GetFilteredVaccinesAsync(
            string petName, string vaccineName, string status, int page, int pageSize = 12);
        Task<GetVaccineByIdDto> GetVaccineByIdAsync(int id);
        Task CreateVaccineAsync(CreateVaccineDto dto);
        Task UpdateVaccineAsync(UpdateVaccineDto dto);
        Task DeleteVaccineAsync(int id);
    }
}
