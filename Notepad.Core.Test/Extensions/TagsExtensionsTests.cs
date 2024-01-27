using Notepad.Core.Extensions;
using Notepad.Core.Models;

namespace Notepad.Core.Test.Extensions
{
    [TestFixture]
    public class TagsExtensionsTests
    {

        [Test]
        public void MapToList_WithPhoneEmail_ShouldReturnCorrectTags()
        {
            var tag = Tag.EMAIL | Tag.PHONE;

            var result = TagsExtensions.MapToList(tag);

            var expected = new List<string> { "EMAIL", "PHONE" };
            CollectionAssert.AreEquivalent(expected, result);
        }

        [Test]
        public void MapToList_WithNone_ShouldReturnEmptyList()
        {
            var tag = Tag.None;

            var result = TagsExtensions.MapToList(tag);

            Assert.IsEmpty(result);
        }

        [Test]
        public void MapToList_WithPhone_ShouldReturnCorrectTags()
        {
            var tag = Tag.PHONE;

            var result = TagsExtensions.MapToList(tag);

            var expected = new List<string> { "PHONE" };
            CollectionAssert.AreEquivalent(expected, result);
        }

        [Test]
        public void MapToList_WithEmail_ShouldReturnCorrectTags()
        {
            var tag = Tag.EMAIL;

            var result = TagsExtensions.MapToList(tag);

            var expected = new List<string> { "EMAIL" };
            CollectionAssert.AreEquivalent(expected, result);
        }
    }
}