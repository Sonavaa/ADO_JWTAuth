
namespace ADO_JWTAuth.DTOs
{
    public class UpdateUserDTO
    {
        public int RoleId { get; set; }

        public string? Email { get; set; }

        public string? Username { get; set; }

        public string? Password { get; set; }
    }
}
