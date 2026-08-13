using ADO_JWTAuth.IRepositories;
using ADO_JWTAuth.Models;

namespace ADO_JWTAuth.Repositories
{
    public class RoleRepository : BaseRepository, IRoleRepository
    {
        public RoleRepository(IConfiguration connectionString) : base(connectionString)
        {
        }

        public async Task<Role?> GetRoleByIdAsync(int Id)
        {
           using var connection = CreateConnection();
            await connection.OpenAsync();

           using var command = connection.CreateCommand();

            command.CommandText = @"SELECT Id,
                                           roleName FROM Roles
                                           WHERE Id = @Id";

            command.Parameters.AddWithValue("@Id", Id);

            using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new Role
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    RoleName = reader.GetString(reader.GetOrdinal("roleName"))
                };
            }
            return null;
        }
    }
}
