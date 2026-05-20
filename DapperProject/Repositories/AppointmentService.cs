using Dapper;
using DapperProject.Context;
using DapperProject.Dtos.AppointmentDtos;

namespace DapperProject.Repositories
{
    public class AppointmentService : IAppointmentService
    {
        private readonly DapperContext _context;

        public AppointmentService(DapperContext context)
        {
            _context = context;
        }

        public async Task<List<ResultAppointmentDto>> GetAllAppointmentsAsync()
        {
            string query = @"
                SELECT a.AppointmentId, p.Name AS PetName, s.SpeciesName,
                       o.FullName AS OwnerName, o.City AS OwnerCity,
                       v.FullName AS VetName, a.AppointmentDate,
                       a.Complaint, a.Status, a.AppointmentFee
                FROM Appointments a
                JOIN Pets p       ON a.PetId = p.PetId
                JOIN Species s    ON p.SpeciesId = s.SpeciesId
                JOIN Owners o     ON p.OwnerId = o.OwnerId
                JOIN Vets v       ON a.VetId = v.VetId";

            var connection = _context.CreateConnection();
            var values = await connection.QueryAsync<ResultAppointmentDto>(query);
            return values.ToList();
        }

        // Filtreleme + sayfalama (Sayfa 3)
        public async Task<(List<ResultAppointmentDto> Data, int TotalCount)> GetFilteredAppointmentsAsync(
            string petName, string status, string city, string vetName, int page, int pageSize = 12)
        {
            // Dinamik WHERE koşulu oluşturulur
            var where = "WHERE 1=1";
            var parameters = new DynamicParameters();

            if (!string.IsNullOrEmpty(petName))
            {
                where += " AND p.Name LIKE @petName";
                parameters.Add("@petName", $"%{petName}%");
            }
            if (!string.IsNullOrEmpty(status))
            {
                where += " AND a.Status = @status";
                parameters.Add("@status", status);
            }
            if (!string.IsNullOrEmpty(city))
            {
                where += " AND o.City = @city";
                parameters.Add("@city", city);
            }
            if (!string.IsNullOrEmpty(vetName))
            {
                where += " AND v.FullName LIKE @vetName";
                parameters.Add("@vetName", $"%{vetName}%");
            }

            parameters.Add("@offset", (page - 1) * pageSize);
            parameters.Add("@pageSize", pageSize);

            string query = $@"
                SELECT a.AppointmentId, p.Name AS PetName, s.SpeciesName,
                       o.FullName AS OwnerName, o.City AS OwnerCity,
                       v.FullName AS VetName, a.AppointmentDate,
                       a.Complaint, a.Status, a.AppointmentFee
                FROM Appointments a
                JOIN Pets p       ON a.PetId = p.PetId
                JOIN Species s    ON p.SpeciesId = s.SpeciesId
                JOIN Owners o     ON p.OwnerId = o.OwnerId
                JOIN Vets v       ON a.VetId = v.VetId
                {where}
                ORDER BY a.AppointmentDate DESC
                OFFSET @offset ROWS FETCH NEXT @pageSize ROWS ONLY";

            string countQuery = $@"
                SELECT COUNT(*) FROM Appointments a
                JOIN Pets p    ON a.PetId = p.PetId
                JOIN Species s ON p.SpeciesId = s.SpeciesId
                JOIN Owners o  ON p.OwnerId = o.OwnerId
                JOIN Vets v    ON a.VetId = v.VetId
                {where}";

            var connection = _context.CreateConnection();
            var data = (await connection.QueryAsync<ResultAppointmentDto>(query, parameters)).ToList();
            var total = await connection.ExecuteScalarAsync<int>(countQuery, parameters);
            return (data, total);
        }


        public async Task<GetAppointmentByIdDto> GetAppointmentByIdAsync(int id)
        {
            string query = "SELECT * FROM Appointments WHERE AppointmentId = @id";
            var parameters = new DynamicParameters();
            parameters.Add("@id", id);
            var connection=_context.CreateConnection();
            return await connection.QueryFirstAsync<GetAppointmentByIdDto>(query, parameters);
        }
        public async Task CreateAppointmentAsync(CreateAppointmentDto dto)
        {
            string query = @"
        INSERT INTO Appointments (AppointmentId, PetId, VetId, AppointmentDate, Complaint, Status, AppointmentFee)
        VALUES ((SELECT ISNULL(MAX(AppointmentId),0)+1 FROM Appointments),
                @petId, @vetId, @appointmentDate, @complaint, @status, @appointmentFee)";
            var parameters = new DynamicParameters();
            parameters.Add("@petId", dto.PetId);
            parameters.Add("@vetId", dto.VetId);
            parameters.Add("@appointmentDate", dto.AppointmentDate);
            parameters.Add("@complaint", dto.Complaint);
            parameters.Add("@status", dto.Status);
            parameters.Add("@appointmentFee", dto.AppointmentFee);

            var connection=_context.CreateConnection();
            await connection.ExecuteAsync(query, parameters);
        }

        public async Task UpdateAppointmentAsync(UpdateAppointmentDto dto)
        {
            string query = @"UPDATE Appointments SET
                PetId = @petId, VetId = @vetId, AppointmentDate = @appointmentDate,
                Complaint = @complaint, Status = @status, AppointmentFee = @appointmentFee
                WHERE AppointmentId = @appointmentId";
            var parameters= new DynamicParameters();
            parameters.Add("@appointmentId", dto.AppointmentId);
            parameters.Add("@petId", dto.PetId);
            parameters.Add("@vetId",dto.VetId);
            parameters.Add("@appointmentDate",dto.AppointmentDate);
            parameters.Add("@complaint",dto.Complaint);
            parameters.Add("@status",dto.Status);
            parameters.Add("@appointmentFee",dto.AppointmentFee);

            var connection=_context.CreateConnection();
            await connection.ExecuteAsync(query, parameters);

        }
      
        public async Task DeleteAppointmentAsync(int id)
        {
            string query = "DELETE FROM Appointments WHERE AppointmentId = @id";
            var parameters= new DynamicParameters();
            parameters.Add("@id", id);
            var connection = _context.CreateConnection();
            await connection.ExecuteAsync(query, parameters);
        }
    }
}
