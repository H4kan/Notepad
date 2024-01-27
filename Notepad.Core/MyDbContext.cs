using Microsoft.EntityFrameworkCore;
using Notepad.Core.Models;

namespace Notepad.Core
{
    public class MyDbContext : DbContext, IMyDbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Note> Notes { get; set; }

        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }

    }
}