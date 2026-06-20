using Microsoft.EntityFrameworkCore;
using Registration.Models;

namespace Registration.Context
{
    public class DbUser : DbContext
    {
        public DbSet<UserRegistrationModel> UserInfo { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB; Database=Users; Trusted_Connection=true");
        }
    }
}
