using ADO_JWTAuth.IServices;
using Microsoft.AspNetCore.Mvc;

namespace ADO_JWTAuth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult> GetRoleById(int Id)
        {
            var role = await _roleService.GetRoleByIdAsync(Id);

            if (role == null)
                return NotFound();

            return Ok(role);
        }
    }
}

