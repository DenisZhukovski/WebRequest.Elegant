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
            Assert.That(
                new UriFromString("http://reqres.in/api/users"),
                Is.EqualTo(new UriFromString("http://reqres.in/api/users"))
            );
        }

        [Test]
        public void NoEqualWhenDifferentUri()
        {
            Assert.That(
                new UriFromString("http://reqres.in/api/users"),
                Is.Not.EqualTo(new UriFromString("http://reqres.in/api/users2"))
            );
        }

        [Test]
        public void EqualToTheSameUri()
        {
            Assert.That(
                new Uri("http://reqres.in/api/users"),
                Is.EqualTo(new UriFromString("http://reqres.in/api/users"))
            );
        }

        [Test]
        public void NotEqualToDiffUri()
        {
            Assert.That(
                new UriFromString("http://reqres.in/api/users"),
                Is.Not.EqualTo(new Uri("http://reqres.in/api/users2"))
            );
        }

        [Test]
        public void EqualToTheSameUriString()
        {
            Assert.That(
                "http://reqres.in/api/users",
                Is.EqualTo(new UriFromString("http://reqres.in/api/users"))
            );
        }

        [Test]
        public void NotEqualToDiffUriString()
        {
            Assert.That(
                new UriFromString("http://reqres.in/api/users"),
                Is.Not.EqualTo("http://reqres.in/api/users2")
            );
        }
    }
}
