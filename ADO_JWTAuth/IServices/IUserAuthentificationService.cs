using ADO_JWTAuth.DTOs;

namespace ADO_JWTAuth.IServices
{
    public interface IUserAuthentificationService
    {
        Task<AuthResponseDTO?> LoginAuth(UserLoginDTO loginDTO);
        Task<AuthResponseDTO?> RefreshTokenAsync(string refreshToken);
    }
}
