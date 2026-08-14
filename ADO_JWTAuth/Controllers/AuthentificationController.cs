using ADO_JWTAuth.DTOs;
using ADO_JWTAuth.IServices;
using Microsoft.AspNetCore.Mvc;

namespace ADO_JWTAuth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthentificationController : ControllerBase
    {
        private readonly IUserAuthentificationService _userAuthentificationService;

        public AuthentificationController(IUserAuthentificationService userAuthentificationService)
        {
                _userAuthentificationService = userAuthentificationService;
        }


        [HttpPost]
        public async Task<IActionResult> Login(UserLoginDTO loginDTO)
        {
            var token = await _userAuthentificationService.LoginAuth(loginDTO);

            if (token == null)
            {
                return Unauthorized(new
                {
                    message = "Username or password is incorrect."
                });
            }

            return Ok(new {
                message = "Login successful.",
                token = token });
        }
    }
}
