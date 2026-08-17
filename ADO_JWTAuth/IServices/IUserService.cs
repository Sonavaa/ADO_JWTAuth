using ADO_JWTAuth.DTOs;
using ADO_JWTAuth.Models;

namespace ADO_JWTAuth.IServices
{
    public interface IUserService
    {
        Task<GetUserDTO> CreateUserAsync(UserDTO createUserDTO);
        Task<List<GetUserDTO>> GetAllUsersAsync();
        Task<GetUserDTO?> GetUserByIdAsync(int Id);
        Task<User?> GetUserByUsernameAsync(string username);
        Task<UpdateUserDTO> UpdateUserAsync(UpdateUserDTO updateUserDTO, int Id);
        Task<bool> DeleteUserAsync(int Id);
        Task SaveRefreshTokenAsync(int userId, string refreshToken, DateTime refreshTokenExpiry);
        Task<User?> GetUserByRefreshTokenAsync(string refreshToken);
    }
}
