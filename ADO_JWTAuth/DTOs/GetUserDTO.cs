
namespace ADO_JWTAuth.DTOs
{
    public class GetUserDTO
    {
        public int RoleId { get; set; }

        public string Email { get; set; } = null!;

        public string Username { get; set; } = null!;

    }
}
