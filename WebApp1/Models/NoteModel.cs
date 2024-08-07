using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp1.Models
{
    [Table("Notes")]  // Specifies that this model maps to the "notes" table in the database
    public class NoteModel
    {
        [Key]
        public int NoteID { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int? AuthorID { get; set; }
        public string Tags { get; set; }
    }
}
