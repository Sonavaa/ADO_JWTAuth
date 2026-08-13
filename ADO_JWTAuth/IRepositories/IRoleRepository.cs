using ADO_JWTAuth.DTOs;
using ADO_JWTAuth.Models;

namespace ADO_JWTAuth.IRepositories
{
    public interface IRoleRepository
    {
        Task<Role?> GetRoleByIdAsync(int Id);
    }
}
