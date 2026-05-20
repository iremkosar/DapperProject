using Dapper;
using DapperProject.Context;
using DapperProject.Dtos.VetDtos;

namespace DapperProject.Repositories
{
    public class VetService : IVetService
    {
        private readonly DapperContext _context;

        public VetService(DapperContext context)
        {
            _context = context;
        }

        public async Task<List<ResultVetDto>> GetAllVetsAsync()
        {
            string query = "SELECT * FROM Vets";
            var connection = _context.CreateConnection();
            var values = await connection.QueryAsync<ResultVetDto>(query);
            return values.ToList();
        }

       
        public async Task<GetVetByIdDto> GetVetByIdAsync(int id)
        {
            string query = "SELECT * FROM Vets WHERE VetId = @id";
            var parameters = new DynamicParameters();
            parameters.Add("@id", id);
            var connection = _context.CreateConnection();
            return await connection.QueryFirstAsync<GetVetByIdDto>(query, parameters);
        }

      
        public async Task CreateVetAsync(CreateVetDto dto)
        {
            string query = @"
        INSERT INTO Vets (VetId, FullName, Specialty, Phone, Email)
        VALUES ((SELECT ISNULL(MAX(VetId),0)+1 FROM Vets), 
                @fullName, @specialty, @phone, @email)";

            var parameters = new DynamicParameters();
            parameters.Add("@fullName", dto.FullName);
            parameters.Add("@specialty", dto.Specialty);
            parameters.Add("@phone", dto.Phone);
            parameters.Add("@email", dto.Email);

            var connection = _context.CreateConnection();
            await connection.ExecuteAsync(query, parameters);
        }

        
        public async Task UpdateVetAsync(UpdateVetDto dto)
        {
            string query = @"UPDATE Vets SET
                FullName = @fullName, Specialty = @specialty,
                Phone = @phone, Email = @email
                WHERE VetId = @vetId";

            var parameters = new DynamicParameters();
            parameters.Add("@vetId", dto.VetId);
            parameters.Add("@fullName", dto.FullName);
            parameters.Add("@specialty", dto.Specialty);
            parameters.Add("@phone", dto.Phone);
            parameters.Add("@email", dto.Email);

            var connection = _context.CreateConnection();
            await connection.ExecuteAsync(query, parameters);
        }

        
        public async Task DeleteVetAsync(int id)
        {
            string query = "DELETE FROM Vets WHERE VetId = @id";
            var parameters = new DynamicParameters();
            parameters.Add("@id", id);
            var connection = _context.CreateConnection();
            await connection.ExecuteAsync(query, parameters);
        }
    }
}
