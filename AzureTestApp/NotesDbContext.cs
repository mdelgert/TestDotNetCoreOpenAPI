using Microsoft.EntityFrameworkCore;
using AzureTestApp.Models;

namespace AzureTestApp
{
    public class NotesDbContext : DbContext
    {
        public NotesDbContext(DbContextOptions<NotesDbContext> options)
            : base(options)
        {
        }

        public DbSet<NoteSqlModel> Notes { get; set; }
    }
}
