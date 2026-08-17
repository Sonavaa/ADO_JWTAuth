using ADO_JWTAuth.DTOs;
using ADO_JWTAuth.IServices;
using Microsoft.AspNetCore.Mvc;

namespace ADO_JWTAuth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IUserAuthentificationService _userAuthentificationService;

        public LoginController(IUserAuthentificationService userAuthentificationService)
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

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken(RefreshTokenDTO dto)
        {
            var result = await _userAuthentificationService.RefreshTokenAsync(dto.RefreshToken);

            if (result == null)
            {
                return Unauthorized("Invalid or expired refresh token.");
            }

            return Ok(result);
        }
    }
}
