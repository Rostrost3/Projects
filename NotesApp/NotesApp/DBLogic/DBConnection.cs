using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NotesApp.Models;

namespace NotesApp.DBLogic
{
    public class DBConnection : DbContext
    {
        public DBConnection(DbContextOptions<DBConnection> options) : base(options) { }

        public DbSet<User> UsersInfo { get; set; }

        public DbSet<Note> Notes { get; set; }
    }
}