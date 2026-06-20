using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;
using System.Text;
using WebApi.Services.Interfaces;

namespace WebApi.Services
{
    public class PasswordHasher : IPasswordHasher
    {
        public string HashPassword(string password)
        {
            using (SHA512 sha512 = SHA512.Create())
            {
                byte[] data = Encoding.UTF8.GetBytes(password);
                byte[] res = sha512.ComputeHash(data);
                return Convert.ToBase64String(res);
            }
        }

        public bool IsPasswordsEqual(string password, string hashPassword)
        {
            return string.Equals(HashPassword(password), hashPassword);
        }
    }
}
