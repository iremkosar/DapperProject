using DapperProject.Dtos.VetDtos;

namespace DapperProject.Repositories
{
    public interface IVetService
    {
        Task<List<ResultVetDto>> GetAllVetsAsync();
        Task<GetVetByIdDto> GetVetByIdAsync(int id);
        Task CreateVetAsync(CreateVetDto createVetDto);
        Task UpdateVetAsync(UpdateVetDto updateVetDto);
        Task DeleteVetAsync(int id);
    }
}
