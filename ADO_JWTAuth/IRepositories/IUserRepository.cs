using ADO_JWTAuth.DTOs;
using ADO_JWTAuth.Models;

namespace ADO_JWTAuth.IRepositories
{
    public interface IUserRepository
    {
        Task<User> CreateUserAsync(UserDTO createUserDTO);
        Task<List<User>> GetAllUsersAsync();
        Task<User?> GetUserByIdAsync(int Id);
        Task<bool> UpdateUserAsync(UserDTO updateUserDTO);
        Task<bool> DeleteUserAsync(int Id);
    }
}
