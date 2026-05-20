using DapperProject.Dtos.PetDtos;

namespace DapperProject.Repositories
{
    public interface IPetService
    {
        Task<List<ResultPetDto>> GetAllPetsAsync();
        Task<GetPetByIdDto> GetPetByIdAsync(int id);
        Task CreatePetAsync(CreatePetDto createPetDto);
        Task UpdatePetAsync(UpdatePetDto updatePetDto);
        Task DeletePetAsync(int id);
        Task<PetDetailDto> GetPetDetailAsync(int id);
        Task<List<PetAppointmentDto>> GetPetAppointmentsAsync(int petId);
        Task<List<PetVaccineDto>> GetPetVaccinesAsync(int petId);
    }
}
