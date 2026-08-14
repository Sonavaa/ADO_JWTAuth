using ADO_JWTAuth.DTOs;
using ADO_JWTAuth.IServices;
using Microsoft.AspNetCore.Authorization;
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

        [HttpPost("create")]
        public async Task<IActionResult> CreateUser(UserDTO userDTO)
        {
            var user = await _userService.CreateUserAsync(userDTO);

            return Ok(new { message = "User Created Successfully!", user });
        }

        [Authorize]
        [HttpGet("list")]
        public async Task<ActionResult<List<UserDTO>>> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();

            return Ok(users);
        }

        //[HttpGet("get{Id:int}")]
        [HttpGet("get/{Id:int}")]
        public async Task<ActionResult> GetUserById(int Id)
        {
            var user = await _userService.GetUserByIdAsync(Id);

            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserDTO updateUserDTO)
        {
            var updatedUser = await _userService.UpdateUserAsync(updateUserDTO);

            return Ok(new
            {
                message = "User updated successfully!",
                user = updatedUser
            });
        }

        //[FromBody]

        [HttpDelete("delete{Id}")]
        public async Task<ActionResult> DeleteUserById(int Id)
        {
            var user = await _userService.DeleteUserAsync(Id);

            if (!user)
                return NotFound();

            return Ok(new { message = "User Deleted Successfully!" });
        }

        [HttpGet("get-by-username/{username}")]
        public async Task<ActionResult> GetUserByUsername(string username)
        {
            var user = await _userService.GetUserByUsernameAsync(username);

            if (user == null)
                return NotFound();

            return Ok(user);
        }
    }
}

//MUST RETURN USER ALREADY DELETED

//ActionResult<T> istifadə etmək daha uyğundur, çünki həm user qaytara bilər, həm də NotFound()