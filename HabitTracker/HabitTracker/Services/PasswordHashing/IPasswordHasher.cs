namespace HabitTracker.Services.PasswordHashing
{
    public interface IPasswordHasher
    {
        (string hash, string salt) HashPassword(string password);

        bool VerifyPassword(string password, string hashPassword, string Salt);
    }
}
