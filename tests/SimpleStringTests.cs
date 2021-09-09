using NUnit.Framework;
using WebRequest.Elegant;

namespace WebRequest.Tests
{
    public class SimpleStringTests
    {
        [Test]
        public void SimpleString_ToJson()
        {
            Assert.AreEqual(
                "test string",
                new SimpleString("test string").ToJson()
            );
        }
    }
}
