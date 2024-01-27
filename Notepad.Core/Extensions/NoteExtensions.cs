using Notepad.Core.Models;

namespace Notepad.Core.Extensions
{
    public static class NoteExtensions
    {
        private static IEnumerable<ITagExtractor> _tagsExtractor = new List<ITagExtractor>()
        {
            new EmailTagExtractor(),
            new PhoneTagExtractor(),
        };

        public static void ExtractTags(this Note note)
        {
            note.Tags = Tag.None;
            foreach (var extractor in  _tagsExtractor)
            {
                extractor.ExtractTag(note);
            }
        } 
    }
}
