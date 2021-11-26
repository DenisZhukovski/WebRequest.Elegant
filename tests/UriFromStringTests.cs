using System;
using NUnit.Framework;
using WebRequest.Elegant.Core;

namespace WebRequest.Tests
{
    public class UriFromStringTests
    {
        [Test]
        public void EqualTheSameUri()
        {
            Assert.AreEqual(
                new UriFromString("http://reqres.in/api/users"),
                new UriFromString("http://reqres.in/api/users")
            );
        }

        [Test]
        public void NoEqualWhenDifferentUri()
        {
            Assert.AreNotEqual(
                new UriFromString("http://reqres.in/api/users"),
                new UriFromString("http://reqres.in/api/users2")
            );
        }

        [Test]
        public void EqualToTheSameUri()
        {
            Assert.AreEqual(
                new UriFromString("http://reqres.in/api/users"),
                new Uri("http://reqres.in/api/users")
            );
        }

        [Test]
        public void NotEqualToDiffUri()
        {
            Assert.AreNotEqual(
                new UriFromString("http://reqres.in/api/users"),
                new Uri("http://reqres.in/api/users2")
            );
        }

        [Test]
        public void EqualToTheSameUriString()
        {
            Assert.AreEqual(
                new UriFromString("http://reqres.in/api/users"),
                "http://reqres.in/api/users"
            );
        }

        [Test]
        public void NotEqualToDiffUriString()
        {
            Assert.AreNotEqual(
                new UriFromString("http://reqres.in/api/users"),
                "http://reqres.in/api/users2"
            );
        }
    }
}
