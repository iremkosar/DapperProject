using Dapper;
using DapperProject.Context;
using DapperProject.Dtos.TreatmentDtos;

namespace DapperProject.Repositories
{
    public class TreatmentService : ITreatmentService
    {
        private readonly DapperContext _context;

        public TreatmentService(DapperContext context)
        {
            _context = context;
        }

        public async Task<(List<ResultTreatmentDto> Data, int TotalCount)> GetFilteredTreatmentsAsync(
            string petName, string diagnosis, string vetName, int page, int pageSize = 12)
        {
            var where = "WHERE 1=1";
            var parameters = new DynamicParameters();

            if (!string.IsNullOrEmpty(petName))
            {
                where += " AND p.Name LIKE @petName";
                parameters.Add("@petName", $"%{petName}%");
            }
            if (!string.IsNullOrEmpty(diagnosis))
            {
                where += " AND t.Diagnosis LIKE @diagnosis";
                parameters.Add("@diagnosis", $"%{diagnosis}%");
            }
            if (!string.IsNullOrEmpty(vetName))
            {
                where += " AND v.FullName LIKE @vetName";
                parameters.Add("@vetName", $"%{vetName}%");
            }

            parameters.Add("@offset", (page - 1) * pageSize);
            parameters.Add("@pageSize", pageSize);

            string query = $@"
                SELECT t.TreatmentId, t.AppointmentId,
                       p.Name AS PetName, s.SpeciesName,
                       o.FullName AS OwnerName, v.FullName AS VetName,
                       t.Diagnosis, t.Notes, t.TreatmentDate, t.TotalCost,
                       (SELECT COUNT(*) FROM Prescriptions pr WHERE pr.TreatmentId = t.TreatmentId) AS PrescriptionCount
                FROM Treatments t
                JOIN Appointments a ON t.AppointmentId = a.AppointmentId
                JOIN Pets p         ON a.PetId = p.PetId
                JOIN Species s      ON p.SpeciesId = s.SpeciesId
                JOIN Owners o       ON p.OwnerId = o.OwnerId
                JOIN Vets v         ON a.VetId = v.VetId
                {where}
                ORDER BY t.TreatmentId DESC
                OFFSET @offset ROWS FETCH NEXT @pageSize ROWS ONLY";

            string countQuery = $@"
                SELECT COUNT(*) FROM Treatments t
                JOIN Appointments a ON t.AppointmentId = a.AppointmentId
                JOIN Pets p         ON a.PetId = p.PetId
                JOIN Species s      ON p.SpeciesId = s.SpeciesId
                JOIN Owners o       ON p.OwnerId = o.OwnerId
                JOIN Vets v         ON a.VetId = v.VetId
                {where}";

            var connection = _context.CreateConnection();
            var data = (await connection.QueryAsync<ResultTreatmentDto>(query, parameters)).ToList();
            var total = await connection.ExecuteScalarAsync<int>(countQuery, parameters);
            return (data, total);
        }

        public async Task<GetTreatmentByIdDto> GetTreatmentByIdAsync(int id)
        {
            string query = @"
                SELECT t.TreatmentId, t.AppointmentId,
                       p.Name AS PetName, s.SpeciesName,
                       o.FullName AS OwnerName, v.FullName AS VetName,
                       t.Diagnosis, t.Notes, t.TreatmentDate, t.TotalCost
                FROM Treatments t
                JOIN Appointments a ON t.AppointmentId = a.AppointmentId
                JOIN Pets p         ON a.PetId = p.PetId
                JOIN Species s      ON p.SpeciesId = s.SpeciesId
                JOIN Owners o       ON p.OwnerId = o.OwnerId
                JOIN Vets v         ON a.VetId = v.VetId
                WHERE t.TreatmentId = @id";

            var parameters = new DynamicParameters();
            parameters.Add("@id", id);
            var connection = _context.CreateConnection();
            return await connection.QueryFirstAsync<GetTreatmentByIdDto>(query, parameters);
        }

        public async Task<List<ResultPrescriptionDto>> GetPrescriptionsByTreatmentIdAsync(int treatmentId)
        {
            string query = @"
                SELECT pr.PrescriptionId, m.MedicineName, m.ActiveIngredient,
                       m.Category, m.UnitPrice, pr.DosageDays,
                       pr.Frequency, pr.Quantity
                FROM Prescriptions pr
                JOIN Medicines m ON pr.MedicineId = m.MedicineId
                WHERE pr.TreatmentId = @treatmentId";

            var parameters = new DynamicParameters();
            parameters.Add("@treatmentId", treatmentId);
            var connection = _context.CreateConnection();
            var values = await connection.QueryAsync<ResultPrescriptionDto>(query, parameters);
            return values.ToList();
        }
    }
}