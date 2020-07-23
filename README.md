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

<h3 align="center">
  [![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=DenisZhukovski_WebRequest.Elegant&metric=alert_status)](https://sonarcloud.io/dashboard?id=DenisZhukovski_WebRequest.Elegant)
  [![Coverage](https://sonarcloud.io/api/project_badges/measure?project=DenisZhukovski_WebRequest.Elegant&metric=coverage)](https://sonarcloud.io/dashboard?id=DenisZhukovski_WebRequest.Elegant)
  [![Duplicated Lines (%)](https://sonarcloud.io/api/project_badges/measure?project=DenisZhukovski_WebRequest.Elegant&metric=duplicated_lines_density)](https://sonarcloud.io/dashboard?id=DenisZhukovski_WebRequest.Elegant)
  [![Maintainability Rating](https://sonarcloud.io/api/project_badges/measure?project=DenisZhukovski_WebRequest.Elegant&metric=sqale_rating)](https://sonarcloud.io/dashboard?id=DenisZhukovski_WebRequest.Elegant)
</h3>


