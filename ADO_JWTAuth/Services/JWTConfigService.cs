using ADO_JWTAuth.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ADO_JWTAuth.Services
{
    public class JWTConfigService
    {
        private readonly JWTConfig _jwtConfig;

        public JWTConfigService(IOptions<JWTConfig> jwtConfig)
        {
            _jwtConfig = jwtConfig.Value;
        }

        public string GenerateToken(string userId, string name)
        {
            var issuer = _jwtConfig.Issuer;
            var audience = _jwtConfig.Audience;
            var expires = DateTime.UtcNow.AddMinutes(_jwtConfig.Expires);
            var key = Encoding.UTF8.GetBytes(_jwtConfig.Key);

            var claims = new List<Claim> {
                new Claim(JwtRegisteredClaimNames.Sub, userId),
                new Claim("name", name),
                new Claim(JwtRegisteredClaimNames.Iss, issuer),
                new Claim(JwtRegisteredClaimNames.Aud, audience)
            };

            var token = new JwtSecurityToken(
                issuer,
                audience,
                claims,
                expires: expires,
                signingCredentials: new SigningCredentials(
                        new SymmetricSecurityKey(key),
                        SecurityAlgorithms.HmacSha256
                    )
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}