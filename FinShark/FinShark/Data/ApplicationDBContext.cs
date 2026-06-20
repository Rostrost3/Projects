using FinShark.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FinShark.Data
{
    public class ApplicationDBContext : IdentityDbContext<AppUser> //Этот класс содержит нужные таблицы(он уже настроил их, то есть связи и т.д.). А т.к. он закрыт AppUser, который наследуется от IdentityUser, то если добавить поле в AppUser, то изменятся и таблицы
    {
        public DbSet<Stock> Stocks { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<Portfolio> Portfolios { get; set; }

        public ApplicationDBContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Portfolio>(x => x.HasKey(p => new { p.AppUserId, p.StockId }));

            builder.Entity<Portfolio>().HasOne(u => u.AppUser).WithMany(u => u.Portfolios).HasForeignKey(p => p.AppUserId);

            builder.Entity<Portfolio>().HasOne(u => u.Stock).WithMany(u => u.Portfolios).HasForeignKey(p => p.StockId);

            List<IdentityRole> roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Id = "1",
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new IdentityRole
                {
                    Id = "2",
                    Name = "User",
                    NormalizedName = "USER"
                }
            };

            builder.Entity<IdentityRole>().HasData(roles);
        }
    }
}
