using ADO_JWTAuth.DTOs;
using System.Security.Cryptography;
using ADO_JWTAuth.IServices;
using ADO_JWTAuth.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ADO_JWTAuth.Services
{
    public class UserAuthentificationService : IUserAuthentificationService
    {
        private readonly IUserService _userService;
        private readonly JWTConfigService _jwtConfigService;
        private readonly PasswordHasher<User> _passwordHasher;

        private string GenerateRefreshToken()
        {
            var randomBytes = new byte[64];

            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);

            return Convert.ToBase64String(randomBytes);
        }

        public UserAuthentificationService(IUserService userService, JWTConfigService jwtConfigService)
        {
                _userService = userService;
                _jwtConfigService = jwtConfigService;
                _passwordHasher = new PasswordHasher<User>();
        }

        public async Task<AuthResponseDTO?> LoginAuth(UserLoginDTO loginDTO)
        {
            var user = await _userService.GetUserByUsernameAsync(loginDTO.Username);

            if (user == null) {
                return null;
            }

            var result = _passwordHasher.VerifyHashedPassword(user, user.Password, loginDTO.Password);

            if (result == PasswordVerificationResult.Failed)
            {
                return null;
            }

            // Access token
            var accessToken = _jwtConfigService.GenerateToken(
                user.Id.ToString(),
                user.Username
            );

            // Refresh token
            var refreshToken = GenerateRefreshToken();

            var refreshTokenExpiry = DateTime.UtcNow.AddDays(7);

            await _userService.SaveRefreshTokenAsync(
                user.Id,
                refreshToken,
                refreshTokenExpiry
            );

            return new AuthResponseDTO
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }

        public async Task<AuthResponseDTO?> RefreshTokenAsync(string refreshToken)
        {
            var user = await _userService.GetUserByRefreshTokenAsync(refreshToken);

            if (user == null)
            {
                return null;
            }

            if (user.RefreshTokenExpiryTime == null ||
                user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                return null;
            }

            var accessToken = _jwtConfigService.GenerateToken(
                user.Id.ToString(),
                user.Username
            );

            return new AuthResponseDTO
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }
    }
}
