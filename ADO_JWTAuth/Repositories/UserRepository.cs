using ADO_JWTAuth.DTOs;
using ADO_JWTAuth.IRepositories;
using ADO_JWTAuth.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ADO_JWTAuth.Repositories
{
    public class UserRepository : DbConnection, IUserRepository
    {
        public UserRepository(IConfiguration connectionString) : base(connectionString)
        {
        }

        public async Task<User> CreateUserAsync(UserDTO userDTO)
        {
            using var connection = CreateConnection();
            await connection.OpenAsync();


            var sqlCommandQuery = @"INSERT INTO Users (Username, Email, Password, RoleId)
                                          VALUES (@Username, @Email, @Password, @RoleId)";

            using var command = CreateCommand(sqlCommandQuery, connection);

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
                IsDeleted = false
            };
        }
        public async Task<List<User>> GetAllUsersAsync()
        {
            using var connection = CreateConnection();
            await connection.OpenAsync();

            var sqlCommandQuery = @"SELECT [Id]
                                           ,[Email]
                                           ,[Username]
                                           ,[RoleId] FROM Users WHERE IsDeleted = 0";

            using var command = CreateCommand(sqlCommandQuery, connection);

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

            var sqlCommandQuery = @"SELECT [Id]
                                           ,[Email]
                                           ,[Username]
                                           ,[RoleId] FROM Users 
                                            WHERE Id=@Id AND IsDeleted = 0";

            using var command = CreateCommand(sqlCommandQuery, connection);

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

        public async Task<User> UpdateUserAsync(UpdateUserDTO updateUserDTO, int Id)
        {
            var existingUser = await GetUserByIdAsync(Id);

            if (existingUser == null)
            { 
                return null;
            }
                

            using var connection = CreateConnection();
            await connection.OpenAsync();

            var sqlCommandQuery = @"UPDATE Users
                                           SET Email = @Email,
                                           Username = @Username,
                                           Password = @Password,
                                           RoleId = @RoleId
                                           WHERE Id = @Id AND IsDeleted = 0";

            using var command = CreateCommand(sqlCommandQuery, connection);

            command.Parameters.AddWithValue("@Email", updateUserDTO.Email);
            command.Parameters.AddWithValue("@Username", updateUserDTO.Username);
            command.Parameters.AddWithValue("@Password", updateUserDTO.Password);
            command.Parameters.AddWithValue("@RoleId", updateUserDTO.RoleId);
            command.Parameters.AddWithValue("@Id", Id);

            await command.ExecuteNonQueryAsync();

            existingUser.Email = updateUserDTO.Email;
            existingUser.Username = updateUserDTO.Username;
            existingUser.Password = updateUserDTO.Password;
            existingUser.RoleId = updateUserDTO.RoleId;

            return existingUser;
        }
      
        public async Task<bool> DeleteUserAsync(int Id)
        {
            using var connection = CreateConnection();
            await connection.OpenAsync();

            var sqlCommandQuery = @"UPDATE Users
                                    SET IsDeleted = 1
                                    WHERE Id = @Id AND IsDeleted = 0";

            using var command = CreateCommand(sqlCommandQuery, connection);

            command.Parameters.AddWithValue("@Id", Id);

            int affectedRows = await command.ExecuteNonQueryAsync();

            return affectedRows > 0;
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            using var connection = CreateConnection();
            await connection.OpenAsync();

            var sqlCommandQuery = @"SELECT [Id]
                                           ,[Email]
                                           ,[Username] 
                                           ,[Password] FROM Users 
                                            WHERE username=@Username AND IsDeleted = 0";

            using var command = CreateCommand(sqlCommandQuery, connection);

            command.Parameters.AddWithValue("@Username", username);

            using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new User
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    Email = reader.GetString(reader.GetOrdinal("Email")),
                    Username = reader.GetString(reader.GetOrdinal("Username")),
                    Password = reader.GetString(reader.GetOrdinal("Password"))
                };

            }
            return null;
        }

        public async Task SaveRefreshTokenAsync(int userId, string refreshToken,
                                                    DateTime refreshTokenExpiry)
        {
            using var connection = CreateConnection();

            await connection.OpenAsync();

            var sqlCommandQuery = @"UPDATE Users
                                    SET RefreshToken = @RefreshToken,
                                        RefreshTokenExpiryTime = @RefreshTokenExpiryTime
                                        WHERE Id = @Id";

            using var command = CreateCommand(sqlCommandQuery, connection);

            command.Parameters.AddWithValue("@RefreshToken", refreshToken);
            command.Parameters.AddWithValue("@RefreshTokenExpiryTime", refreshTokenExpiry);
            command.Parameters.AddWithValue("@Id", userId);

            await command.ExecuteNonQueryAsync();
        }

        public async Task<User?> GetUserByRefreshTokenAsync(string refreshToken)
        {
            using var connection = CreateConnection();
            await connection.OpenAsync();

            var sqlCommandQuery = @"SELECT Id, Username, Email, Password, RoleId,
                                   RefreshToken, RefreshTokenExpiryTime
                            FROM Users
                            WHERE RefreshToken = @RefreshToken
                              AND IsDeleted = 0";

            using var command = CreateCommand(sqlCommandQuery, connection);

            command.Parameters.AddWithValue("@RefreshToken", refreshToken);

            using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new User
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    Username = reader.GetString(reader.GetOrdinal("Username")),
                    Email = reader.GetString(reader.GetOrdinal("Email")),
                    Password = reader.GetString(reader.GetOrdinal("Password")),
                    RoleId = reader.GetInt32(reader.GetOrdinal("RoleId")),
                    RefreshToken = reader.GetString(reader.GetOrdinal("RefreshToken")),
                    RefreshTokenExpiryTime = reader.GetDateTime(
                        reader.GetOrdinal("RefreshTokenExpiryTime"))
                };
            }

            return null;
        }
    }
}
