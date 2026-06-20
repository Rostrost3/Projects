namespace WebApi.Services.Interfaces
{
    public interface IPasswordHasher
    {
        public string HashPassword(string password);

        public bool IsPasswordsEqual(string password, string hashPassword);
    }
}
