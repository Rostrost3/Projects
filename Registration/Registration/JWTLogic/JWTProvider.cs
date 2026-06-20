using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Registration.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Registration.JWTLogic
{
    public class JWTProvider
    {
        private readonly JWTOptions _options;

        public JWTProvider(IOptions<JWTOptions> options)
        {
            _options = options.Value;
        }

        public string GenerateToken(LoginModel user)
        {
            Claim[] claims = [new("userMail", user.Mail.ToString())];

            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey)),
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                signingCredentials: signingCredentials,
                expires: DateTime.UtcNow.AddHours(_options.ExpiresHours)
                );

            var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenValue.ToString();
        }
    }
}
