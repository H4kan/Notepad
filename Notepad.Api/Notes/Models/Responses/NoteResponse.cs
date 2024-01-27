namespace Notepad.Api.Notes.Models.Responses
{
    public class NoteResponse
    {
        public int NoteId { get; set; }

        public string Content { get; set; } = string.Empty;

        public IEnumerable<string> Tags { get; set; } = new List<string>();
    }
}
