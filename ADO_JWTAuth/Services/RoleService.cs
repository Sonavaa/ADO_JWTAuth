using ADO_JWTAuth.DTOs;
using ADO_JWTAuth.IRepositories;
using ADO_JWTAuth.IServices;
using ADO_JWTAuth.Models;
using ADO_JWTAuth.Repositories;
using Microsoft.AspNetCore.Identity;

namespace ADO_JWTAuth.Services
{
    public class RoleService : IRoleService
    {

        private readonly IRoleRepository _repository;

        public RoleService(IRoleRepository repository)
        {
            _repository = repository;
        }

       public async Task<RoleDTO?> GetRoleByIdAsync(int Id)
       {
            if (Id <= 0)
                throw new ArgumentException("Invalid Role Id.");

            var role = await _repository.GetRoleByIdAsync(Id);

            if (role == null)
            {
                return null;
            }

            var roleDTOById = new RoleDTO
            {

                Id = role.Id,
                RoleName = role.RoleName
            };
                return roleDTOById;
       }
    }
}
