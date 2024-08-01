using WebApp1.Models;
using Microsoft.EntityFrameworkCore;

namespace WebApp1
{
    public class NotesDbContext : DbContext
    {
        public NotesDbContext(DbContextOptions<NotesDbContext> options)
            : base(options)
        {
        }

        public DbSet<NoteModel> Notes { get; set; }
    }
}
