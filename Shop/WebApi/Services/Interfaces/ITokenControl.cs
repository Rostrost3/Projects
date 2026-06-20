using DTO.Entities;

namespace WebApi.Services.Interfaces
{
    public interface ITokenControl
    {
        public AuthResponse CreateToken(int userId);
    }
}
