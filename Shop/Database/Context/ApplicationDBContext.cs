using Database.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Database.Context
{
    public class ApplicationDBContext : DbContext
    {
        public DbSet<Product> Products { get; set; }

        public DbSet<Cart> Cart { get; set; }

        public DbSet<CartItem> CartItems { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Role> Roles { get; set; }

        public ApplicationDBContext() : base(new DbContextOptionsBuilder<ApplicationDBContext>().UseSqlServer("Server=Rostrost\\SQLEXPRESS;Database=Shop;Trusted_Connection=True;TrustServerCertificate=True;").Options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, RoleName = "Admin" },
                new Role { Id = 2, RoleName = "User" }
            );
        }
    }
}