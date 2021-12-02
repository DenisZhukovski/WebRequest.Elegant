# WebRequest.Elegant
<h3 align="center">
   
  [![NuGet](https://img.shields.io/nuget/v/WebRequest.Elegant.svg)](https://www.nuget.org/packages/WebRequest.Elegant/) 
  [![Downloads](https://img.shields.io/nuget/dt/WebRequest.Elegant.svg)](https://www.nuget.org/WebRequest.Elegant/)
  [![Stars](https://img.shields.io/github/stars/DenisZhukovski/WebRequest.Elegant?color=brightgreen)](https://github.com/DenisZhukovski/WebRequest.Elegant/stargazers) 
  [![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE.md) 
  [![Hits-of-Code](https://hitsofcode.com/github/deniszhukovski/webrequest.elegant)](https://hitsofcode.com/view/github/deniszhukovski/webrequest.elegant)
  [![EO principles respected here](https://www.elegantobjects.org/badge.svg)](https://www.elegantobjects.org)
  [![PDD status](https://www.0pdd.com/svg?name=deniszhukovski/webrequest.elegant)](https://www.0pdd.com/p?name=deniszhukovski/webrequest.elegant)
</h3>

The main idea is to wrap HttpClient type with more elegant and object oriented entity. The entity provides immutable objects by cloning itself and able to make requests to different end points. It's assumed that developers create the WebRequest entity only once in the app at startup and then clonning it in other places to complete the reuqest.

```cs
var server = new WebRequest("http://some.server.com"); // An application creates the WebRequest only once and then reuses it.
```
Once it has been created developers should inject it as a constructor argument to all the entities that may need it.

```cs
public class Users
{
  private IWebRequest _server;
  public Users(IWebRequest server)
  {
    _server = server;
  }
  
  public async Task<IList<User>> ToListAsync()
  {
    var usersRequest = server.WithRelativePath("/users"); // new WebRequest object will be created and 
                                                          // refers to http://some.server.com/users
    var usersResponse = await usersRequest.GetResponseAsync();
    return ... // parse the resposne and create list of users
  }
}
```

The main goal of this approach is to become a staple component for the SDKs that are built like tree structure.

## Useful extension
This enxention method is useful when its needed to deserialize the response into an object. It was not added into the original package because JsonConvert class creates dependency on 3rd party component but it's been decided not to pin on any 3rd party libraries.

```cs
public static class WebRequestExtensions
{
   public static async Task<T> ReadAsync<T>(this IWebRequest request)
   {
      string content = string.Empty;
      try
      {
         content = await request
            .ReadAsStringAsync()
            .ConfigureAwait(false);
         if (typeof(T) == typeof(string))
         {
            return (T)(object)content;
         }

         return JsonConvert.DeserializeObject<T>(content);
      }
      catch (Exception ex)
      {
         var invalidOperationException = new InvalidOperationException(
            $"Web Request error occured for {request}",
            ex
         );
         invalidOperationException.Data["Content"] = content;
         throw invalidOperationException;
      }
   }

   public static IWebRequest WithBody(this IWebRequest request, JObject body)
   {
      return request.WithBody(new SimpleString(body.ToString()));
   }

   public static async Task<IList<T>> SelectAsync<T>(
      this IWebRequest request,
      Func<JObject, T> map)
   {
      var response = await request
         .ReadAsync<List<JObject>>()
         .ConfigureAwait(false);

      return response.Select(map).ToList();
   }
}
```
## In Unit Tests
To help developers in writing unit tests WebRequest.Elegant package contains some useful classes to assist.
<br/>
[FkHttpMessageHandler](https://github.com/DenisZhukovski/WebRequest.Elegant/blob/master/src/Fakes/FkHttpMessageHandler.cs) can be used to fake all responses from the real server. All the requests' responses will be mocked with configured one.
```cs
await new Elegant.WebRequest(
   "http://reqres.in/api/users",
   new FkHttpMessageHandler("Response message as a text here")
).UploadFileAsync(filePath);
```
[RoutedHttpMessageHandler](https://github.com/DenisZhukovski/WebRequest.Elegant/blob/master/src/Fakes/RoutedHttpMessageHandler.cs) can be used to fake a real server responses for particular URIs. All the requests' responses will be mocked by route map.
### Important:
[RoutedHttpMessageHandler](https://github.com/DenisZhukovski/WebRequest.Elegant/blob/master/src/Fakes/RoutedHttpMessageHandler.cs) will pass the request to original [HttpClientHandler](https://docs.microsoft.com/en-us/dotnet/api/system.net.http.httpclienthandler?view=net-6.0) when no configured route is found.
```cs
Assert.AreEqual(
   "Hello world",
   await new Elegant.WebRequest(
      new Uri("http://reqres.in/api/users"),
      new RoutedHttpMessageHandler(
         new Route(new Dictionary<string, string>
         {
            // It's key value pair where key is uri to be mocked and value is a message that will be responded.
            { "http://reqres.in/api/users", "Hello world" }
         })
      )
    ).ReadAsStringAsync()
);
```
[ProxyHttpMessageHandler](https://github.com/DenisZhukovski/WebRequest.Elegant/blob/master/src/Fakes/ProxyHttpMessageHandler.cs) can be used to proxy all the requests/responses.
```cs
var proxy = new ProxyHttpMessageHandler();
await new Elegant.WebRequest(
   new Uri("https://www.google.com/"),
   proxy
).ReadAsStringAsync()

Assert.IsNotEmpty(proxy.RequestsContent);
Assert.IsNotEmpty(proxy.ResponsesContent);
```
## Equal to
Despite the fact that [WebRequest](https://github.com/DenisZhukovski/WebRequest.Elegant/blob/master/src/WebRequest.cs) entity tries to encapsulate internal state it still provides smart Equal method:
```cs
Assert.AreEqual(
   new Elegant.WebRequest(new Uri("http://reqres.in/api/users")),
   new Uri("http://reqres.in/api/users")
);

Assert.AreEqual(
   new Elegant.WebRequest(new Uri("http://reqres.in/api/users")),
   "http://reqres.in/api/users"
);

Assert.AreEqual(
   new Elegant.WebRequest(
      new Uri("http://reqres.in/api/users")
   ).WithMethod(HttpMethod.Post),
   HttpMethod.Post
);
```

## Build status

<div align="center">
  
   [![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=DenisZhukovski_WebRequest.Elegant&metric=alert_status)](https://sonarcloud.io/dashboard?id=DenisZhukovski_WebRequest.Elegant) 
   [![Coverage](https://sonarcloud.io/api/project_badges/measure?project=DenisZhukovski_WebRequest.Elegant&metric=coverage)](https://sonarcloud.io/dashboard?id=DenisZhukovski_WebRequest.Elegant)
   [![Duplicated Lines (%)](https://sonarcloud.io/api/project_badges/measure?project=DenisZhukovski_WebRequest.Elegant&metric=duplicated_lines_density)](https://sonarcloud.io/dashboard?id=DenisZhukovski_WebRequest.Elegant)
   [![Maintainability Rating](https://sonarcloud.io/api/project_badges/measure?project=DenisZhukovski_WebRequest.Elegant&metric=sqale_rating)](https://sonarcloud.io/dashboard?id=DenisZhukovski_WebRequest.Elegant) 
</div>


