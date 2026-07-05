using HabitTracker.DataBase.Entities;
using Microsoft.EntityFrameworkCore;

namespace HabitTracker.DataBase
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Habit> Habits { get; set; }

        public DbSet<User> Users { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique(true);
        }
    }
}
