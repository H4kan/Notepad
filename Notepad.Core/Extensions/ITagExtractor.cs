using Notepad.Core.Models;

namespace Notepad.Core.Extensions
{
    public interface ITagExtractor
    {
        void ExtractTag(Note note);
    }
}
