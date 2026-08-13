using ADO_JWTAuth.DTOs;
using ADO_JWTAuth.Models;

namespace ADO_JWTAuth.IServices
{
    public interface IRoleService
    {
        Task<RoleDTO?> GetRoleByIdAsync(int Id);
    }
}
