using DapperProject.Dtos.OwnerDtos;

namespace DapperProject.Repositories
{
    public interface IOwnerService
    {
        Task<List<ResultOwnerDto>> GetAllOwnersAsync();
        Task<GetOwnerByIdDto> GetOwnerByIdAsync(int id);
        Task CreateOwnerAsync(CreateOwnerDto createOwnerDto);
        Task UpdateOwnerAsync(UpdateOwnerDto updateOwnerDto);
        Task DeleteOwnerAsync(int id);
        Task<OwnerDetailDto> GetOwnerDetailAsync(int id);
        Task<List<OwnerPetDto>> GetOwnerPetsAsync(int ownerId);
        Task<List<OwnerAppointmentDto>> GetOwnerAppointmentsAsync(int ownerId);
    }
}
