using Dapper;
using DapperProject.Context;
using DapperProject.Dtos.PetDtos;

namespace DapperProject.Repositories
{
    public class PetService:IPetService
    {
        private readonly DapperContext _context;

        public PetService(DapperContext context)
        {
            _context = context;
        }

        public async Task<List<ResultPetDto>> GetAllPetsAsync()
        {
            string query = @"
                SELECT p.PetId, p.Name, s.SpeciesName, b.BreedName,
                       o.FullName AS OwnerName, p.Gender,
                       p.BirthDate, p.IsNeutered, p.Weight
                FROM Pets p
                JOIN Species s ON p.SpeciesId = s.SpeciesId
                JOIN Breeds b  ON p.BreedId = b.BreedId
                JOIN Owners o  ON p.OwnerId = o.OwnerId";

            var connection = _context.CreateConnection();
            var values = await connection.QueryAsync<ResultPetDto>(query);
            return values.ToList();
        }

      
        public async Task<GetPetByIdDto> GetPetByIdAsync(int id)
        {
            string query = "SELECT * FROM Pets WHERE PetId = @id";
            var parameters = new DynamicParameters();
            parameters.Add("@id", id);
            var connection = _context.CreateConnection();
            return await connection.QueryFirstAsync<GetPetByIdDto>(query, parameters);
        }

      
        public async Task CreatePetAsync(CreatePetDto dto)
        {
            string query = @"
    INSERT INTO Pets (PetId, OwnerId, SpeciesId, BreedId, Name, Gender, BirthDate, IsNeutered, Weight)
    VALUES ((SELECT ISNULL(MAX(PetId),0)+1 FROM Pets),
            @ownerId, @speciesId, @breedId, @name, @gender, @birthDate, @isNeutered, @weight)";

            var parameters = new DynamicParameters();
            parameters.Add("@ownerId", dto.OwnerId);
            parameters.Add("@speciesId", dto.SpeciesId);
            parameters.Add("@breedId", dto.BreedId);
            parameters.Add("@name", dto.Name);
            parameters.Add("@gender", dto.Gender);
            parameters.Add("@birthDate", dto.BirthDate);
            parameters.Add("@isNeutered", dto.IsNeutered);
            parameters.Add("@weight", dto.Weight);

            var connection = _context.CreateConnection();
            await connection.ExecuteAsync(query, parameters);
        }

       
        public async Task UpdatePetAsync(UpdatePetDto dto)
        {
            string query = @"UPDATE Pets SET
                OwnerId = @ownerId, SpeciesId = @speciesId, BreedId = @breedId,
                Name = @name, Gender = @gender, BirthDate = @birthDate,
                IsNeutered = @isNeutered, Weight = @weight
                WHERE PetId = @petId";

            var parameters = new DynamicParameters();
            parameters.Add("@petId", dto.PetId);
            parameters.Add("@ownerId", dto.OwnerId);
            parameters.Add("@speciesId", dto.SpeciesId);
            parameters.Add("@breedId", dto.BreedId);
            parameters.Add("@name", dto.Name);
            parameters.Add("@gender", dto.Gender);
            parameters.Add("@birthDate", dto.BirthDate);
            parameters.Add("@isNeutered", dto.IsNeutered);
            parameters.Add("@weight", dto.Weight);

            var connection = _context.CreateConnection();
            await connection.ExecuteAsync(query, parameters);
        }

      
        public async Task DeletePetAsync(int id)
        {
            string query = "DELETE FROM Pets WHERE PetId = @id";
            var parameters = new DynamicParameters();
            parameters.Add("@id", id);
            var connection = _context.CreateConnection();
            await connection.ExecuteAsync(query, parameters);
        }
        public async Task<PetDetailDto> GetPetDetailAsync(int id)
        {
            string query = @"
        SELECT p.PetId, p.Name, s.SpeciesName, b.BreedName,
               o.FullName AS OwnerName, o.Phone AS OwnerPhone,
               p.Gender, p.BirthDate, p.IsNeutered, p.Weight,
               (SELECT COUNT(*) FROM Appointments a WHERE a.PetId = p.PetId) AS TotalAppointments,
               (SELECT COUNT(*) FROM Vaccines v WHERE v.PetId = p.PetId) AS TotalVaccines,
               (SELECT COUNT(*) FROM Treatments t 
                JOIN Appointments a ON t.AppointmentId = a.AppointmentId 
                WHERE a.PetId = p.PetId) AS TotalTreatments,
               (SELECT ISNULL(SUM(t.TotalCost),0) FROM Treatments t
                JOIN Appointments a ON t.AppointmentId = a.AppointmentId
                WHERE a.PetId = p.PetId) AS TotalSpent
        FROM Pets p
        JOIN Species s ON p.SpeciesId = s.SpeciesId
        JOIN Breeds b  ON p.BreedId = b.BreedId
        JOIN Owners o  ON p.OwnerId = o.OwnerId
        WHERE p.PetId = @id";

            var parameters = new DynamicParameters();
            parameters.Add("@id", id);
            var connection = _context.CreateConnection();
            return await connection.QueryFirstAsync<PetDetailDto>(query, parameters);
        }

        public async Task<List<PetAppointmentDto>> GetPetAppointmentsAsync(int petId)
        {
            string query = @"
        SELECT TOP 10
            a.AppointmentId, v.FullName AS VetName,
            a.AppointmentDate, a.Complaint,
            a.Status, a.AppointmentFee
        FROM Appointments a
        JOIN Vets v ON a.VetId = v.VetId
        WHERE a.PetId = @petId
        ORDER BY TRY_CONVERT(DATETIME, a.AppointmentDate, 120) DESC";

            var parameters = new DynamicParameters();
            parameters.Add("@petId", petId);
            var connection = _context.CreateConnection();
            var values = await connection.QueryAsync<PetAppointmentDto>(query, parameters);
            return values.ToList();
        }

        public async Task<List<PetVaccineDto>> GetPetVaccinesAsync(int petId)
        {
            string query = @"
        SELECT v.VaccineId, v.VaccineName,
               vet.FullName AS VetName,
               v.VaccineDate, v.NextDueDate, v.BatchNo
        FROM Vaccines v
        JOIN Vets vet ON v.VetId = vet.VetId
        WHERE v.PetId = @petId
        ORDER BY v.VaccineDate DESC";

            var parameters = new DynamicParameters();
            parameters.Add("@petId", petId);
            var connection = _context.CreateConnection();
            var values = await connection.QueryAsync<PetVaccineDto>(query, parameters);
            return values.ToList();
        }
    }
}
