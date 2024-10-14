using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using JwtAuthenticationApp.JwtConfig;
using System.Security.Claims;
using System.Text;

namespace JwtAuthenticationApp.Services
{
    public class JwtTokenService(ILogger<JwtTokenService> logger)
    {
        public string GetJwtTokenString(string id, string userName, string[] userRoles)
        {
            var claims = new List<Claim>()
            {
                new("Id", id.ToString()),
                new(JwtRegisteredClaimNames.Sub, userName),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            claims.AddRange(userRoles.Select(role =>
                new Claim(ClaimsIdentity.DefaultRoleClaimType, role)));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(60), // Refactor - remove magic number
                Issuer = CommonJwtConfiguration.ValidIssuer,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(CommonJwtConfiguration.SecurityKey)), SecurityAlgorithms.HmacSha256)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenToString = tokenHandler.WriteToken(token);

            logger.LogInformation("JWT Token for user {} (ID: {}) created", userName, id);

            return tokenToString;
        }
    }
}
