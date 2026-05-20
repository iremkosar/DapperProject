using Dapper;
using DapperProject.Context;
using DapperProject.Dtos.OwnerDtos;

namespace DapperProject.Repositories
{
    public class OwnerService:IOwnerService
    {
        private readonly DapperContext _context;

        public OwnerService(DapperContext context)
        {
            _context = context;
        }

        public async Task CreateOwnerAsync(CreateOwnerDto dto)
        {
            string query = @"
        INSERT INTO Owners (OwnerId, FullName, Gender, Phone, Email, City, RegisteredAt)
        VALUES ((SELECT ISNULL(MAX(OwnerId),0)+1 FROM Owners),
                @fullName, @gender, @phone, @email, @city, @registeredAt)";

            var parameters = new DynamicParameters();
            parameters.Add("@fullName", dto.FullName);
            parameters.Add("@gender", dto.Gender);
            parameters.Add("@phone", dto.Phone);
            parameters.Add("@email", dto.Email);
            parameters.Add("@city", dto.City);
            parameters.Add("@registeredAt", DateTime.Now.ToString("yyyy-MM-dd"));

            var connection=_context.CreateConnection();
            await connection.ExecuteAsync(query, parameters);
        }

        public async Task DeleteOwnerAsync(int id)
        {
            string query = "DELETE FROM Owners WHERE OwnerId = @id";
            var parameters = new DynamicParameters();
            parameters.Add("@id", id);
            var connection=_context.CreateConnection();
            await connection.ExecuteAsync(query, parameters);
        }

        public async Task<List<ResultOwnerDto>> GetAllOwnersAsync()
        {
            string query = @"
    SELECT OwnerId, FullName, Gender, Phone, Email, City,
           CONVERT(NVARCHAR(10), RegisteredAt, 23) AS RegisteredAt
    FROM Owners";
            var connection=_context.CreateConnection();
            var values = await connection.QueryAsync<ResultOwnerDto>(query);
            return values.ToList();
        }

        public async Task<GetOwnerByIdDto> GetOwnerByIdAsync(int id)
        {
            string query = "SELECT * FROM Owners Where OwnerId=@id";
            var parameters=new DynamicParameters();
            parameters.Add("@id", id);
            var connection=_context.CreateConnection();
            return await connection.QueryFirstAsync<GetOwnerByIdDto>(query, parameters);
        }

        public async Task UpdateOwnerAsync(UpdateOwnerDto dto)
        {
            string query = @"UPDATE Owners SET
                FullName = @fullName, Gender = @gender, Phone = @phone,
                Email = @email, City = @city
                WHERE OwnerId = @ownerId";
            var parameters = new DynamicParameters();
            parameters.Add("@ownerId", dto.OwnerId);
            parameters.Add("@fullName", dto.FullName);
            parameters.Add("@gender", dto.Gender);
            parameters.Add("@phone", dto.Phone);
            parameters.Add("@email", dto.Email);
            parameters.Add("@city", dto.City);

            var connection = _context.CreateConnection();
            await connection.ExecuteAsync(query, parameters);
        }
        public async Task<OwnerDetailDto> GetOwnerDetailAsync(int id)
        {
            string query = @"
        SELECT o.OwnerId, o.FullName, o.Gender, o.Phone, o.Email, o.City,
               CONVERT(NVARCHAR(10), o.RegisteredAt, 23) AS RegisteredAt,
               (SELECT COUNT(*) FROM Pets p WHERE p.OwnerId = o.OwnerId) AS TotalPets,
               (SELECT COUNT(*) FROM Appointments a 
                JOIN Pets p ON a.PetId = p.PetId 
                WHERE p.OwnerId = o.OwnerId) AS TotalAppointments,
               (SELECT ISNULL(SUM(t.TotalCost),0) FROM Treatments t
                JOIN Appointments a ON t.AppointmentId = a.AppointmentId
                JOIN Pets p ON a.PetId = p.PetId
                WHERE p.OwnerId = o.OwnerId) AS TotalSpent
        FROM Owners o
        WHERE o.OwnerId = @id";

            var parameters = new DynamicParameters();
            parameters.Add("@id", id);
            var connection = _context.CreateConnection();
            return await connection.QueryFirstAsync<OwnerDetailDto>(query, parameters);
        }

        public async Task<List<OwnerPetDto>> GetOwnerPetsAsync(int ownerId)
        {
            string query = @"
        SELECT p.PetId, p.Name, s.SpeciesName, b.BreedName,
               p.Gender, p.BirthDate, p.IsNeutered, p.Weight,
               (SELECT COUNT(*) FROM Appointments a WHERE a.PetId = p.PetId) AS AppointmentCount
        FROM Pets p
        JOIN Species s ON p.SpeciesId = s.SpeciesId
        JOIN Breeds b  ON p.BreedId = b.BreedId
        WHERE p.OwnerId = @ownerId";

            var parameters = new DynamicParameters();
            parameters.Add("@ownerId", ownerId);
            var connection = _context.CreateConnection();
            var values = await connection.QueryAsync<OwnerPetDto>(query, parameters);
            return values.ToList();
        }

        public async Task<List<OwnerAppointmentDto>> GetOwnerAppointmentsAsync(int ownerId)
        {
            string query = @"
        SELECT TOP 10
            a.AppointmentId, p.Name AS PetName,
            v.FullName AS VetName, a.AppointmentDate,
            a.Complaint, a.Status, a.AppointmentFee
        FROM Appointments a
        JOIN Pets p  ON a.PetId = p.PetId
        JOIN Vets v  ON a.VetId = v.VetId
        WHERE p.OwnerId = @ownerId
        ORDER BY TRY_CONVERT(DATETIME, a.AppointmentDate, 120) DESC";

            var parameters = new DynamicParameters();
            parameters.Add("@ownerId", ownerId);
            var connection = _context.CreateConnection();
            var values = await connection.QueryAsync<OwnerAppointmentDto>(query, parameters);
            return values.ToList();
        }
    }
}
