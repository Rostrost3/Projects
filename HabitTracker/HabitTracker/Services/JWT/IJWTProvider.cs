namespace HabitTracker.Services.JWT
{
    public interface IJWTProvider
    {
        string GenerateToken(int id, string userName);
    }
}
