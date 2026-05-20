using DapperProject.Dtos.AppointmentDtos;

namespace DapperProject.Repositories
{
    public interface IAppointmentService
    {
        Task<List<ResultAppointmentDto>> GetAllAppointmentsAsync();

        // Filtreleme ve sayfalama ile randevuları listele
        Task<(List<ResultAppointmentDto> Data, int TotalCount)> GetFilteredAppointmentsAsync(
            string petName, string status, string city, string vetName, int page, int pageSize = 12);

        // ID'ye göre tek randevu getirir (Güncelleme formu için)
        Task<GetAppointmentByIdDto> GetAppointmentByIdAsync(int id);
       
        Task CreateAppointmentAsync(CreateAppointmentDto createAppointmentDto);
      
        Task UpdateAppointmentAsync(UpdateAppointmentDto updateAppointmentDto);
      
        Task DeleteAppointmentAsync(int id);





    }
}
