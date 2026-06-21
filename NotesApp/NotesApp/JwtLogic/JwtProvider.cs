using Azure;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NotesApp.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NotesApp.JwtLogic
{
    public class JwtProvider
    {
        private readonly JwtOptions jwtOptions;

        public JwtProvider(IOptions<JwtOptions> _jwtOptions)
        {
            jwtOptions = _jwtOptions.Value;
        }

        public string Generate(string email)
        {
            Claim[] claims = [new("UserMail", email)];

            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey)),
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                signingCredentials: signingCredentials,
                expires: DateTime.UtcNow.AddHours(jwtOptions.ExpiresHours));

            var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenValue.ToString();
        }

        public void SetAuthCookie(HttpResponse response, string email)
        {
            var token = Generate(email);
            response.Cookies.Append("strange-cookie", token);
        }
    }
}
