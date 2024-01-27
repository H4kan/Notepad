using Notepad.Core.Extensions;
using Notepad.Core.Models;

namespace Notepad.Core.Test.Extensions
{
    [TestFixture]
    public class PhoneTagExtractorTests
    {
        private PhoneTagExtractor _extractor;

        [SetUp]
        public void Setup()
        {
            _extractor = new PhoneTagExtractor();
        }

        [Test]
        public void ExtractTag_WithValidPhoneNumber_SetsPhoneTag()
        {
     
            var note = new Note { Content = "Call me at +15852826520." };

     
            _extractor.ExtractTag(note);

       
            Assert.IsTrue(note.Tags.HasFlag(Tag.PHONE));
        }

        [Test]
        public void ExtractTag_WithValidPhoneNumber_SetsPhoneTag2()
        {

            var note = new Note { Content = "Call me at +48601223456" };


            _extractor.ExtractTag(note);


            Assert.IsTrue(note.Tags.HasFlag(Tag.PHONE));
        }

        [Test]
        public void ExtractTag_WithInvalidPhoneNumber_DoesNotSetPhoneTag()
        {

            var note = new Note { Content = "This is just a random text." };

     
            _extractor.ExtractTag(note);

    
            Assert.IsFalse(note.Tags.HasFlag(Tag.PHONE));
        }
    }
}