namespace Registration.Models
{
    public class JWTOptions
    {
        public string SecretKey { get; set; } = string.Empty;

        public int ExpiresHours { get; set; }
    }
}
