using NUnit.Framework;
using WebRequest.Elegant;

namespace WebRequest.Tests
{
    public class SimpleStringTests
    {
        [Test]
        public void SimpleString_ToJson()
        {
            Assert.That(
                new SimpleString("test string").ToJson(),
                Is.EqualTo("test string")
            );
        }
    }
}
