using ADO_JWTAuth.DTOs;
using ADO_JWTAuth.IRepositories;
using ADO_JWTAuth.IServices;
using ADO_JWTAuth.Models;
using ADO_JWTAuth.Repositories;
using Microsoft.AspNetCore.Identity;

namespace ADO_JWTAuth.Services
{
    public class UserService : IUserService
    {

        private readonly IUserRepository _repository;

        public UserService(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<UserDTO> CreateUserAsync(UserDTO userDTO)
        {
            if (userDTO.RoleId <= 0)
                throw new Exception("RoleId is required.");

            if (string.IsNullOrWhiteSpace(userDTO.Username))
                throw new Exception("Username is required.");

            if (string.IsNullOrWhiteSpace(userDTO.Email))
                throw new Exception("Email is required.");

            if (string.IsNullOrWhiteSpace(userDTO.Password))
                throw new Exception("Password is required.");



            var passwordHasher = new PasswordHasher<User>();

            var user = new User
            {
                Username = userDTO.Username,
                Email = userDTO.Email,
                RoleId = userDTO.RoleId
            };

            user.Password = passwordHasher.HashPassword(user, userDTO.Password);

            userDTO.Password = user.Password;

            user = await _repository.CreateUserAsync(userDTO);

            var newUserDTOWithHashedPassword = new UserDTO
            {
                Username = user.Username,
                Email = user.Email,
                Password = user.Password,
                RoleId = user.RoleId
            };

            return new UserDTO
            {
                Username = newUserDTOWithHashedPassword.Username,
                Email = newUserDTOWithHashedPassword.Email,
                Password = newUserDTOWithHashedPassword.Password,
                RoleId = newUserDTOWithHashedPassword.RoleId,
            };
        }

        public async Task<List<UserDTO>> GetAllUsersAsync()
        {
           var users = await _repository.GetAllUsersAsync();

           var userDTOs = new List<UserDTO>();


            foreach (var user in users)
            {
                //Console.WriteLine($"Id: {user.Id}");
                //Console.WriteLine($"RoleId: {user.RoleId}");
                //Console.WriteLine($"Email: {user.Email}");
                //Console.WriteLine($"Username: {user.Username}");

                userDTOs.Add(new UserDTO
                {
                    RoleId = user.RoleId,
                    Email = user.Email,
                    Username = user.Username
                });
            }

            return userDTOs;
        }

        public async Task<UserDTO?> GetUserByIdAsync(int Id)
        {
            var user = await _repository.GetUserByIdAsync(Id);

            if (user == null)
            {
                return null;
            }

            var userDTOById = new UserDTO();

            userDTOById.RoleId = user.RoleId;
            userDTOById.Email = user.Email;
            userDTOById.Username = user.Username;

            return userDTOById; 
        }

        public async Task<bool> UpdateUserAsync(UserDTO updateUserDTO)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteUserAsync(int Id)
        {
            if (Id == 0)
                return false;

            return await _repository.DeleteUserAsync(Id);
        }
    }
}
