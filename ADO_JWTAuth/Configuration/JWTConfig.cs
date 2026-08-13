namespace ADO_JWTAuth.Configuration
{
    public class JWTConfig
    {
        public string Issuer { get; set; } = null!;
        public string Audience { get; set; } = null!;
        public int Expires { get; set; } 
        public string Key { get; set; } = null!;
    }
}
