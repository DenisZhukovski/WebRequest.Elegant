using System;
using System.Net.Http;
using System.Threading.Tasks;
using NUnit.Framework;
using WebRequest.Elegant;
using WebRequest.Elegant.Fakes;

namespace WebRequest.Tests;

public class ConditionalResponseTests : BaseResponseTests
{
    public ConditionalResponseTests()
        : base(
            ConditionalResponse(), 
            "TestRequest",
            "Success")
    {
        
    }
    
    [Test]
    public  Task NotMatched_WhenRequestIsDifferent()
    {
        return Assert.ThatAsync(
            () => ConditionalResponse().MatchesAsync(new HttpRequestMessage() { Content = new StringContent("WrongRequest")}),
            Is.EqualTo(false)
        );
    }

    private static ConditionalResponse ConditionalResponse()
    {
        return new ConditionalResponse(
            message => message.ContainsAsync("TestRequest"),
            new StringResponse("Success")
        );
    }
}