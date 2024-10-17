using JwtAuthenticationApp.Models;
using JwtGlobalConfiguration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JwtAuthenticationApp.Services
{
    public class JwtTokenService(ILogger<JwtTokenService> logger)
    {
        public string GetJwtTokenString(UserModel user)
        {
            var claims = new List<Claim>()
            {
                new("Id", user.Id),
                new(JwtRegisteredClaimNames.Sub, user.Login),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            claims.AddRange(user.Roles.Select(role =>
                new Claim(ClaimsIdentity.DefaultRoleClaimType, role)));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(JwtConfiguration.ExpirationTimeInMinutes),
                Issuer = JwtConfiguration.ValidIssuer,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(JwtConfiguration.SecurityKey)), SecurityAlgorithms.HmacSha256)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenToString = tokenHandler.WriteToken(token);

            logger.LogInformation("JWT Token for user {} (ID: {}) created", user.Login, user.Id);

            return tokenToString;
        }
    }
}
