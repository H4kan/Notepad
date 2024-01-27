using Notepad.Core.Models;


namespace Notepad.Core.Extensions
{
    public static class TagsExtensions
    {
        public static List<string> MapToList(this Tag tag)
        {
            var tags = new List<string>();

            foreach (Tag tagEnum in Enum.GetValues(typeof(Tag)))
            {
                if (tagEnum != Tag.None && (tag & tagEnum) == tagEnum)
                {
                    tags.Add(tagEnum.ToString());
                }
            }

            return tags;
        }

    }
}
