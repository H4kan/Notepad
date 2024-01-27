using Notepad.Core.Extensions;
using Notepad.Core.Models;

namespace Notepad.Core.Test.Extensions
{
    [TestFixture]
    public class EmailTagExtractorTests
    {
        private EmailTagExtractor _extractor;

        [SetUp]
        public void Setup()
        {
            _extractor = new EmailTagExtractor();
        }

        [Test]
        public void ExtractTag_WithValidEmail_SetsEmailTag()
        {
     
            var note = new Note { Content = "Write me at ss@gmail.com" };

     
            _extractor.ExtractTag(note);

       
            Assert.IsTrue(note.Tags.HasFlag(Tag.EMAIL));
        }

        [Test]
        public void ExtractTag_WithValidEmail_SetsEmailTag2()
        {

            var note = new Note { Content = "Write me at support@my-ap.com" };


            _extractor.ExtractTag(note);


            Assert.IsTrue(note.Tags.HasFlag(Tag.EMAIL));
        }

        [Test]
        public void ExtractTag_WithInvalidEmail_DoesNotSetEmailTag()
        {

            var note = new Note { Content = "This is just a random text." };

     
            _extractor.ExtractTag(note);

    
            Assert.IsFalse(note.Tags.HasFlag(Tag.EMAIL));
        }
    }
}