using ADO_JWTAuth.DTOs;
using ADO_JWTAuth.IRepositories;
using ADO_JWTAuth.IServices;
using ADO_JWTAuth.Models;
using ADO_JWTAuth.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace ADO_JWTAuth.Services
{
    public class UserService : IUserService
    {

        private readonly IUserRepository _repository;
        private readonly IRoleRepository _roleRepository;
        private readonly PasswordHasher<User> _passwordHasher;
        private readonly IMapper _mapper;

        public UserService(IUserRepository repository, IRoleRepository roleRepository, PasswordHasher<User> passwordHasher, IMapper mapper)
        {
            _repository = repository;
            _roleRepository = roleRepository;
            _passwordHasher = passwordHasher;
            _mapper = mapper;
        }

        public async Task<GetUserDTO> CreateUserAsync(UserDTO userDTO)
        {
            var user = new User
            {
                Username = userDTO.Username,
                Email = userDTO.Email,
                RoleId = userDTO.RoleId
            };

            user.Password = _passwordHasher.HashPassword(user, userDTO.Password);

            userDTO.Password = user.Password;

            var newUserDTO = new UserDTO
            {
                Username = user.Username,
                Email = user.Email,
                Password = user.Password,
                RoleId = user.RoleId
            };

            var createdUser = await _repository.CreateUserAsync(newUserDTO);

            return _mapper.Map<GetUserDTO>(user);
        }

        public async Task<List<GetUserDTO>> GetAllUsersAsync()
        {
           var users = await _repository.GetAllUsersAsync();

            return _mapper.Map<List<GetUserDTO>>(users);
        }

        public async Task<GetUserDTO?> GetUserByIdAsync(int Id)
        {
            var user = await _repository.GetUserByIdAsync(Id);

            if (user == null)
            {
                return null;
            }

            return _mapper.Map<GetUserDTO>(user);
        }

        public async Task<UpdateUserDTO> UpdateUserAsync(UpdateUserDTO updateUserDTO, int Id)
        {
            if (updateUserDTO == null)
                throw new ArgumentNullException(nameof(updateUserDTO));

            if (Id <= 0)
                throw new ArgumentException("Invalid user Id.");

            var existUser = await _repository.GetUserByIdAsync(Id);

            if (existUser == null)
                throw new KeyNotFoundException("User not found.");

            existUser.RoleId = updateUserDTO.RoleId;

            var role = await _roleRepository.GetRoleByIdAsync(updateUserDTO.RoleId);

            if (role == null)
                throw new KeyNotFoundException("Role not found.");

            var updatedHashedPassword = _passwordHasher.HashPassword(existUser, updateUserDTO.Password);
            updateUserDTO.Password = updatedHashedPassword;

            var updatedUser = await _repository.UpdateUserAsync(updateUserDTO, Id);

            if (updatedUser.Password is not null)
            {
                updatedUser.Password = updatedHashedPassword;
            }

            return _mapper.Map<UpdateUserDTO>(updatedUser);

        }

        public async Task<bool> DeleteUserAsync(int Id)
        {
            if (Id <= 0)
                return false;

            return await _repository.DeleteUserAsync(Id);
        }

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

            //return _mapper.Map<User>(user);
        }

        public async Task SaveRefreshTokenAsync(int userId, string refreshToken,
                                                            DateTime refreshTokenExpiry)
        {
            await _repository.SaveRefreshTokenAsync(
                userId,
                refreshToken,
                refreshTokenExpiry);
        }

        public async Task<User?> GetUserByRefreshTokenAsync(string refreshToken)
        {
            return await _repository.GetUserByRefreshTokenAsync(refreshToken);
        }
    }
}
