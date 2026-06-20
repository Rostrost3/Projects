using Microsoft.AspNetCore.Identity;

namespace FinShark.Models
{
    public class AppUser : IdentityUser //IdentityUser содержит часто используемые параметры. В AppUser можем добавить ещё свои
    {
        public List<Portfolio> Portfolios { get; set; } = new List<Portfolio>();
    }
}
