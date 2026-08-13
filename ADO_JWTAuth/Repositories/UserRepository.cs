using ADO_JWTAuth.DTOs;
using ADO_JWTAuth.IRepositories;
using ADO_JWTAuth.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ADO_JWTAuth.Repositories
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        public UserRepository(IConfiguration connectionString) : base(connectionString)
        {
        }

        public async Task<User> CreateUserAsync(UserDTO userDTO)
        {
            using var connection = CreateConnection();
            await connection.OpenAsync();

            using var command = connection.CreateCommand();

            command.CommandText = @"INSERT INTO Users (Username, Email, Password, RoleId)
                                          VALUES (@Username, @Email, @Password, @RoleId";

            command.Parameters.AddWithValue("@Username", userDTO.Username);
            command.Parameters.AddWithValue("@Email", userDTO.Email);
            command.Parameters.AddWithValue("@Password", userDTO.Password);
            command.Parameters.AddWithValue("@RoleId", userDTO.RoleId);

            await command.ExecuteNonQueryAsync();

            return new User
            {
                Username = userDTO.Username,
                Email = userDTO.Email,
                Password = userDTO.Password,
                RoleId = userDTO.RoleId,
            };
        }
        public async Task<List<User>> GetAllUsersAsync()
        {
            using var connections = CreateConnection();
            await connections.OpenAsync();

            using var command = connections.CreateCommand();

            command.CommandText = @"SELECT [Id]
                                           ,[Email]
                                           ,[Username]
                                           ,[RoleId] FROM Users";

            using var reader = await command.ExecuteReaderAsync();
            
            var users = new List<User>();

            while (await reader.ReadAsync()) {
                var user = new User
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    RoleId = reader.GetInt32(reader.GetOrdinal("RoleId")),
                    Email = reader.GetString(reader.GetOrdinal("Email")),
                    Username = reader.GetString(reader.GetOrdinal("Username"))
                };

                users.Add(user);
            }

            return users;
        }

        public async Task<User?> GetUserByIdAsync(int Id)
        {
          using var connection = CreateConnection();
            await connection.OpenAsync();

          using var command = connection.CreateCommand();

            command.CommandText = @"SELECT [Id]
                                           ,[Email]
                                           ,[Username]
                                           ,[RoleId] FROM Users 
                                            WHERE Id=@Id";

            command.Parameters.AddWithValue("@Id", Id);

            using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync()) {
                return new User
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    RoleId = reader.GetInt32(reader.GetOrdinal("RoleId")),
                    Email = reader.GetString(reader.GetOrdinal("Email")),
                    Username = reader.GetString(reader.GetOrdinal("Username"))
                };

            }
            return null;
        }

        public Task<bool> UpdateUserAsync(UserDTO updateUserDTO)
        {
            throw new NotImplementedException();
        }
      
        public async Task<bool> DeleteUserAsync(int Id)
        {
            using var connection = CreateConnection();
            await connection.OpenAsync();

            using var command = connection.CreateCommand();

            command.CommandText = @"UPDATE Users
                                    SET IsDeleted = 1
                                    WHERE Id = @Id";

            command.Parameters.AddWithValue("@Id", Id);

            int affectedRows = await command.ExecuteNonQueryAsync();

            return affectedRows > 0;
        }


    }
}
