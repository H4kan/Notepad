using System.ComponentModel.DataAnnotations;

namespace Notepad.Core.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        public string Username { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public ICollection<Note> Notes { get; set; } = new HashSet<Note>();
    }
}
