using Notepad.Core.Models;
using PhoneNumbers;
using System.Text.RegularExpressions;

namespace Notepad.Core.Extensions
{
    public class PhoneTagExtractor : ITagExtractor
    {
        private static PhoneNumberUtil _phoneNumberUtil = PhoneNumberUtil.GetInstance();

        public void ExtractTag(Note note)
        {
            if (_phoneNumberUtil.FindNumbers(note.Content, null).Any()) {
                note.Tags |= Tag.PHONE;
            }
        }
    }
}
