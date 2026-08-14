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
        private readonly IRoleRepository _roleRepository;

        public UserService(IUserRepository repository, IRoleRepository roleRepository)
        {
            _repository = repository;
            _roleRepository = roleRepository;
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

            var newUserDTO = new UserDTO
            {
                Username = user.Username,
                Email = user.Email,
                Password = user.Password,
                RoleId = user.RoleId
            };

            var createdUser = await _repository.CreateUserAsync(newUserDTO);

            return new UserDTO
            {
                Username = newUserDTO.Username,
                Email = newUserDTO.Email,
                RoleId = newUserDTO.RoleId,
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

        public async Task<UserDTO> UpdateUserAsync(UpdateUserDTO updateUserDTO)
        { 
            var Id = updateUserDTO.Id;

            if (Id <= 0)
                throw new ArgumentException("Invalid user Id.");

            if (updateUserDTO == null)
                throw new ArgumentNullException(nameof(updateUserDTO));

            var existUser = await _repository.GetUserByIdAsync(Id);

            if (existUser == null)
                throw new KeyNotFoundException("User not found.");

            var role = await _roleRepository.GetRoleByIdAsync((int)updateUserDTO.RoleId);

            if (role == null)
                throw new KeyNotFoundException("Role not found.");

            var updatedUser = await _repository.UpdateUserAsync(updateUserDTO, Id);

            return new UserDTO
            {
                Username = updatedUser.Username,
                Email = updatedUser.Email,
                RoleId = updatedUser.RoleId
            };

        }

        public async Task<bool> DeleteUserAsync(int Id)
        {
            if (Id <= 0)
                return false;

            return await _repository.DeleteUserAsync(Id);
        }


        // Niyə UserLoginDTO tipinden olmaz?
        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            var user = await _repository.GetUserByUsernameAsync(username);

            if (user == null)
            {
                return null;
            }

            var userDTOByusername = new User();

            userDTOByusername.Id = user.Id;
            userDTOByusername.Username = user.Username;
            userDTOByusername.Password = user.Password;

            return userDTOByusername;
        }

        public async Task SaveRefreshTokenAsync(int userId, string refreshToken,
                                                            DateTime refreshTokenExpiry)
        {
            await _repository.SaveRefreshTokenAsync(
                userId,
                refreshToken,
                refreshTokenExpiry);
        }
    }
}
