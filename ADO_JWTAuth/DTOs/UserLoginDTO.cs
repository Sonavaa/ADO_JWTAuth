namespace ADO_JWTAuth.DTOs
{
    public class UserLoginDTO
    {
        public int? Id { get; set; } 
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
