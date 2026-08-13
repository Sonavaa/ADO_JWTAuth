using ADO_JWTAuth.DTOs;
using ADO_JWTAuth.Models;

namespace ADO_JWTAuth.IRepositories
{
    public interface IUserRepository
    {
        Task<User> CreateUserAsync(UserDTO createUserDTO);
        Task<List<User>> GetAllUsersAsync();
        Task<User?> GetUserByIdAsync(int Id);
        Task<User> UpdateUserAsync(UpdateUserDTO updateUserDTO, int Id);
        Task<bool> DeleteUserAsync(int Id);
    }
}
