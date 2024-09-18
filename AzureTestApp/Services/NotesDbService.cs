using Microsoft.EntityFrameworkCore;
using AzureTestApp.Models;

namespace AzureTestApp.Services
{
    public class NotesDbService : DbContext
    {
        public NotesDbService(DbContextOptions<NotesDbService> options)
            : base(options)
        {
        }

        public DbSet<NoteSqlModel> Notes { get; set; }
    }
}
