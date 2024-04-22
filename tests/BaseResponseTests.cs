using System;
using System.Net.Http;
using System.Threading.Tasks;
using NUnit.Framework;
using WebRequest.Elegant;
using WebRequest.Elegant.Fakes;

namespace WebRequest.Tests;

public abstract class BaseResponseTests
{
    private readonly IResponse _response;
    private readonly HttpRequestMessage _requestMessage;
    private readonly string _responseMessage;

    protected BaseResponseTests(IResponse response, string requestMessage, string responseMessage)
        : this(response, new HttpRequestMessage() { Content = new StringContent(requestMessage)}, responseMessage)
    {
    }

    protected BaseResponseTests(IResponse response, HttpRequestMessage requestMessage, string responseMessage)
    {
        _response = response;
        _requestMessage = requestMessage;
        _responseMessage = responseMessage;
    }
    
    [Test]
    public async Task WebRequest_WithTheResponse()
    {
        var result = await new Elegant.WebRequest(
            new Uri("http://reqres.in/api/users"),
            new RoutedHttpMessageHandler(
                new Route(
                    _response
                )
            )
        )
        .WithBody(
            await _requestMessage.Content.ReadAsStringAsync()
        )
        .ReadAsStringAsync();
        Assert.That(
            () => result,
            Is.EqualTo(_responseMessage)
        );
    }

    [Test]
    public Task MatchesAsync()
    {
        return Assert.ThatAsync(
            () => _response.MatchesAsync(_requestMessage),
            Is.EqualTo(true)
        );
    }

    [Test]
    public async Task MessageForAsync()
    {
        var response = await _response.MessageForAsync(_requestMessage);
        await Assert.ThatAsync(
            () => response.Content.ReadAsStringAsync(),
            Is.EqualTo(_responseMessage)
        );
    }
}