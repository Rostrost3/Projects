namespace HabitTracker.Models
{
    public class JWTOptions
    {
        public string SecretKey { get; set; } = string.Empty;

        public double ExpireHours { get; set; }

        public string Issuer { get; set; } = string.Empty;

        public string Audience { get; set; } = string.Empty;
    }
}
