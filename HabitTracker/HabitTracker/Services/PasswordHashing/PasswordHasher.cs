using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using System.Text;

namespace HabitTracker.Services.PasswordHashing
{
    public class PasswordHasher : IPasswordHasher
    {
        private const int SaltSize = 16;
        private const int HashSize = 32;
        private const int Iterations = 100000;

        private string GenerateHash(string password, byte[] salt)
        {
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: Iterations,
                numBytesRequested: HashSize
            ));
        }

        public (string hash, string salt) HashPassword(string password)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);
            string hashed = GenerateHash(password, salt);
            return (hashed, Convert.ToBase64String(salt));
        }

        public bool VerifyPassword(string password, string hashPassword, string salt)
        {
            byte[] saltByte = Convert.FromBase64String(salt);
            string hashed = GenerateHash(password, saltByte);
            return hashed.Equals(hashPassword);
        }
    }
}
