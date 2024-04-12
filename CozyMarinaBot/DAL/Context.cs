using CozyMarinaBot.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace CozyMarinaBot.DAL
{
    internal sealed class Context : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Word> Words { get; set; }
        public Context(DbContextOptions<Context> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
        public Context()
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=/app/db/CozyMarinaBot.db");
        }
    }
}
