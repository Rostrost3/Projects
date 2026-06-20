using Database.Entities;
using Database.Repositories;
using DTO.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApi.Services.Interfaces;

namespace WebApi.Services
{
    public class TokenControl : ITokenControl
    {
        private readonly IConfiguration _configuration;

        private readonly IRepository<User> _userRep;

        private readonly IRepository<Role> _roleRep;

        public TokenControl(IConfiguration configuration, IRepository<User> userRep, IRepository<Role> roleRep)
        {
            _configuration = configuration;
            _userRep = userRep;
            _roleRep = roleRep;
        }

        public AuthResponse CreateToken(int userId)
        {
            var user = _userRep.GetByFunc(x => x.Id == userId);
            var role = _roleRep.GetByFunc(x => x.Id == user!.RoleId);

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                    new Claim(ClaimTypes.Role, role!.RoleName)
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetValue<string>("JWTSecret")!)), SecurityAlgorithms.HmacSha512Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenGenerated = tokenHandler.WriteToken(token);
            AuthResponse authResponse = new() { Token = tokenGenerated };
            return authResponse;
        }
    }
}
