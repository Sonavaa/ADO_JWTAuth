
namespace ADO_JWTAuth.DTOs
{
    public class UserDTO
    {
        public int RoleId { get; set; }

        public string Email { get; set; } = null!;

        public string Username { get; set; } = null!;

        public string Password { get; set; } = null!;
        public bool IsDeleted { get; set; } = false;

    }
}
