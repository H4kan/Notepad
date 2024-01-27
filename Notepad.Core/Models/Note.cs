
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Notepad.Core.Models
{
    public class Note
    {
        [Key]
        public int Id { get; set; }

        public string Content { get; set; } = string.Empty;

        [ForeignKey("User")]
        public int UserId { get; set; }

        public User User { get; set; } = new User();

        public Tag Tags { get; set; } = Tag.None;
    }
}
