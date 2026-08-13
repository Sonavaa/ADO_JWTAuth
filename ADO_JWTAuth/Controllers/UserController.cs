using ADO_JWTAuth.DTOs;
using ADO_JWTAuth.IServices;
using Microsoft.AspNetCore.Mvc;

namespace ADO_JWTAuth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(UserDTO userDTO)
        {
            var user = await _userService.CreateUserAsync(userDTO);

            return Ok(new { message = "User Created Successfully!", user });
        }

        [HttpGet]
        public async Task<ActionResult<List<UserDTO>>> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();

            return Ok(users);
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult> GetUserById(int Id)
        {
            var user = await _userService.GetUserByIdAsync(Id);

            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> UpdateUser( int Id, UpdateUserDTO updateUserDTO)
        {
            var updatedUser = await _userService.UpdateUserAsync(updateUserDTO, Id);

            return Ok(new
            {
                message = "User updated successfully!",
                user = updatedUser
            });
        }

        //[FromBody]

        [HttpDelete("{Id}")]
        public async Task<ActionResult> DeleteUserById(int Id)
        {
            var user = await _userService.DeleteUserAsync(Id);

            if (!user)
                return NotFound();

            return Ok(new { message = "User Deleted Successfully!" });
        }
    }
}

//MUST RETURN USER ALREADY DELETED

//ActionResult<T> istifadə etmək daha uyğundur, çünki həm user qaytara bilər, həm də NotFound()