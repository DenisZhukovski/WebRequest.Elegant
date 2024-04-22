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

    private static IResponse StringResponse()
    {
        return new StringResponse(
            "Success"
        );
    }
}