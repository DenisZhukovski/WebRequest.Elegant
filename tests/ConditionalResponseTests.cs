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

    [Test]
    public async Task Match_WhenConditionIsTrue()
    {
        Assert.That(
            await new Elegant.WebRequest(
                    new Uri("http://reqres.in/api/users"),
                    new RoutedHttpMessageHandler(
                        new Route(
                            new ConditionalResponse(message => message.ContainsAsync("First"), new StringResponse("Incorrect")),
                            new ConditionalResponse(message => message.ContainsAsync("Second"), new StringResponse("Correct"))
                        )
                    )
                )
                .WithBody("Second condition should be matched")
                .ReadAsStringAsync(),
            Is.EqualTo("Correct")
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