using Microsoft.EntityFrameworkCore;
using Notepad.Core.Models;

namespace Notepad.Core
{
    public interface IMyDbContext
    {
        DbSet<User> Users { get; }

        public DbSet<Note> Notes { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
