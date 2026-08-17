using ADO_JWTAuth.DTOs;
using ADO_JWTAuth.IServices;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ADO_JWTAuth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IValidator<UserDTO> _validator;
        private readonly IValidator<UpdateUserDTO> _updateValidator;

        public UserController(IUserService userService, IValidator<UserDTO> validator, IValidator<UpdateUserDTO> updateValidator)
        {
            _userService = userService;
            _validator = validator;
            _updateValidator = updateValidator;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateUser(UserDTO userDTO)
        {
            var validationResult = await _validator.ValidateAsync(userDTO);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var user = await _userService.CreateUserAsync(userDTO);

            return Ok(new { message = "User Created Successfully!", user });
        }

        [Authorize]
        [HttpGet("list")]
        public async Task<ActionResult<List<GetUserDTO>>> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();

            return Ok(users);
        }

        [HttpGet("get/{Id:int}")]
        public async Task<ActionResult<GetUserDTO>> GetUserById(int Id)
        {
            var user = await _userService.GetUserByIdAsync(Id);

            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpPut("update/{Id:int}")]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserDTO updateUserDTO, int Id)
        {
            var validationResult = await _updateValidator.ValidateAsync(updateUserDTO);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var updatedUser = await _userService.UpdateUserAsync(updateUserDTO, Id);

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
            {
                return NotFound();
            }

            return Ok(user);
        }
    }
}

//ActionResult<T> istifadə etmək daha uyğundur, çünki həm user qaytara bilər, həm də NotFound()