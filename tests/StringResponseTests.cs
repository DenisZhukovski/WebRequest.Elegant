using System;
using System.Net.Http;
using System.Threading.Tasks;
using NUnit.Framework;
using WebRequest.Elegant;
using WebRequest.Elegant.Fakes;

namespace WebRequest.Tests;

public class StringResponseTests : BaseResponseTests
{
    public StringResponseTests()
        : base(
            StringResponse(), 
            "",
            "Success")
    {
        
    }
    
    [Test]
    public  Task AlwaysMatched()
    {
        return Assert.ThatAsync(
            () => StringResponse().MatchesAsync(new HttpRequestMessage() { Content = new StringContent("WrongRequest")}),
            Is.EqualTo(true)
        );
    }

    [Test]
    public async Task AlwaysAppliedFirstResponse()
    {
        Assert.That(
            await new Elegant.WebRequest(
                    new Uri("http://reqres.in/api/users"),
                    new RoutedHttpMessageHandler(
                        new Route(
                            new StringResponse("First response"),
                            new StringResponse("Second response")
                        )
                    )
                )
                .ReadAsStringAsync(),
            Is.EqualTo("First response")
        );
    }

    private static StringResponse StringResponse()
    {
        return new StringResponse(
            "Success"
        );
    }
}