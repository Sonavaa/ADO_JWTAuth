using ADO_JWTAuth.DTOs;
using ADO_JWTAuth.Models;

namespace ADO_JWTAuth.IServices
{
    public interface IUserService
    {
        Task<UserDTO> CreateUserAsync(UserDTO createUserDTO);
        Task<List<UserDTO>> GetAllUsersAsync();
        Task<UserDTO?> GetUserByIdAsync(int Id);
        Task<User?> GetUserByUsernameAsync(string username);
        Task<UserDTO> UpdateUserAsync(UpdateUserDTO updateUserDTO);
        Task<bool> DeleteUserAsync(int Id);
        Task SaveRefreshTokenAsync(int userId, string refreshToken, DateTime refreshTokenExpiry);
    }
}
