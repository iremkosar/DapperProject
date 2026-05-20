using Dapper;
using DapperProject.Context;
using DapperProject.Dtos.VaccineDtos;

namespace DapperProject.Repositories
{
    public class VaccineService : IVaccineService
    {
        private readonly DapperContext _context;

        public VaccineService(DapperContext context)
        {
            _context = context;
        }

        public async Task<(List<ResultVaccineDto> Data, int TotalCount)> GetFilteredVaccinesAsync(
            string petName, string vaccineName, string status, int page, int pageSize = 12)
        {
            var where = "WHERE 1=1";
            var parameters = new DynamicParameters();

            if (!string.IsNullOrEmpty(petName))
            {
                where += " AND p.Name LIKE @petName";
                parameters.Add("@petName", $"%{petName}%");
            }
            if (!string.IsNullOrEmpty(vaccineName))
            {
                where += " AND v.VaccineName LIKE @vaccineName";
                parameters.Add("@vaccineName", $"%{vaccineName}%");
            }
            // Gecikmiş filtresi
            if (status == "Gecikmiş")
            {
                where += " AND TRY_CONVERT(DATE, v.NextDueDate, 23) < CAST(GETDATE() AS DATE)";
            }
            else if (status == "Yaklaşan")
            {
                where += @" AND TRY_CONVERT(DATE, v.NextDueDate, 23) 
                           BETWEEN CAST(GETDATE() AS DATE) 
                           AND DATEADD(DAY, 30, CAST(GETDATE() AS DATE))";
            }
            else if (status == "Normal")
            {
                where += " AND TRY_CONVERT(DATE, v.NextDueDate, 23) > DATEADD(DAY, 30, CAST(GETDATE() AS DATE))";
            }

            parameters.Add("@offset", (page - 1) * pageSize);
            parameters.Add("@pageSize", pageSize);

            string query = $@"
                SELECT v.VaccineId, p.Name AS PetName, s.SpeciesName,
                       o.FullName AS OwnerName, o.Phone AS OwnerPhone,
                       vet.FullName AS VetName, v.VaccineName,
                       v.VaccineDate, v.NextDueDate, v.BatchNo
                FROM Vaccines v
                JOIN Pets p     ON v.PetId = p.PetId
                JOIN Species s  ON p.SpeciesId = s.SpeciesId
                JOIN Owners o   ON p.OwnerId = o.OwnerId
                JOIN Vets vet   ON v.VetId = vet.VetId
                {where}
                ORDER BY v.NextDueDate ASC
                OFFSET @offset ROWS FETCH NEXT @pageSize ROWS ONLY";

            string countQuery = $@"
                SELECT COUNT(*) FROM Vaccines v
                JOIN Pets p    ON v.PetId = p.PetId
                JOIN Species s ON p.SpeciesId = s.SpeciesId
                JOIN Owners o  ON p.OwnerId = o.OwnerId
                JOIN Vets vet  ON v.VetId = vet.VetId
                {where}";

            var connection = _context.CreateConnection();
            var data = (await connection.QueryAsync<ResultVaccineDto>(query, parameters)).ToList();
            var total = await connection.ExecuteScalarAsync<int>(countQuery, parameters);
            return (data, total);
        }

        public async Task<GetVaccineByIdDto> GetVaccineByIdAsync(int id)
        {
            string query = "SELECT * FROM Vaccines WHERE VaccineId = @id";
            var parameters = new DynamicParameters();
            parameters.Add("@id", id);
            var connection = _context.CreateConnection();
            return await connection.QueryFirstAsync<GetVaccineByIdDto>(query, parameters);
        }

        public async Task CreateVaccineAsync(CreateVaccineDto dto)
        {
            string query = @"
                INSERT INTO Vaccines (VaccineId, PetId, VetId, VaccineName, VaccineDate, NextDueDate, BatchNo)
                VALUES ((SELECT ISNULL(MAX(VaccineId),0)+1 FROM Vaccines),
                        @petId, @vetId, @vaccineName, @vaccineDate, @nextDueDate, @batchNo)";

            var parameters = new DynamicParameters();
            parameters.Add("@petId", dto.PetId);
            parameters.Add("@vetId", dto.VetId);
            parameters.Add("@vaccineName", dto.VaccineName);
            parameters.Add("@vaccineDate", dto.VaccineDate);
            parameters.Add("@nextDueDate", dto.NextDueDate);
            parameters.Add("@batchNo", dto.BatchNo);

            var connection = _context.CreateConnection();
            await connection.ExecuteAsync(query, parameters);
        }

        public async Task UpdateVaccineAsync(UpdateVaccineDto dto)
        {
            string query = @"
                UPDATE Vaccines SET
                    PetId = @petId, VetId = @vetId, VaccineName = @vaccineName,
                    VaccineDate = @vaccineDate, NextDueDate = @nextDueDate, BatchNo = @batchNo
                WHERE VaccineId = @vaccineId";

            var parameters = new DynamicParameters();
            parameters.Add("@vaccineId", dto.VaccineId);
            parameters.Add("@petId", dto.PetId);
            parameters.Add("@vetId", dto.VetId);
            parameters.Add("@vaccineName", dto.VaccineName);
            parameters.Add("@vaccineDate", dto.VaccineDate);
            parameters.Add("@nextDueDate", dto.NextDueDate);
            parameters.Add("@batchNo", dto.BatchNo);

            var connection = _context.CreateConnection();
            await connection.ExecuteAsync(query, parameters);
        }

        public async Task DeleteVaccineAsync(int id)
        {
            string query = "DELETE FROM Vaccines WHERE VaccineId = @id";
            var parameters = new DynamicParameters();
            parameters.Add("@id", id);
            var connection = _context.CreateConnection();
            await connection.ExecuteAsync(query, parameters);
        }
    }
}