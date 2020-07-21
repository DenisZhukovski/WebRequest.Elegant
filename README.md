<h3 align="center">
  
  [![NuGet](https://img.shields.io/nuget/v/WebRequest.Elegant.svg)](https://www.nuget.org/packages/WebRequest.Elegant/)
  [![Downloads](https://img.shields.io/nuget/dt/WebRequest.Elegant.svg)](https://www.nuget.org/WebRequest.Elegant/)
  [![Stars](https://img.shields.io/github/stars/DenisZhukovski/WebRequest.Elegant?color=brightgreen)](https://github.com/DenisZhukovski/WebRequest.Elegant/stargazers)
  [![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE.md)
  [![Hits-of-Code](https://hitsofcode.com/github/deniszhukovski/webrequest.elegant)](https://hitsofcode.com/view/github/deniszhukovski/webrequest.elegant)
  [![EO principles respected here](https://www.elegantobjects.org/badge.svg)](https://www.elegantobjects.org)
  [![PDD status](https://www.0pdd.com/svg?name=deniszhukovski/webrequest.elegant)](https://www.0pdd.com/p?name=deniszhukovski/webrequest.elegant)
  [![Maintainability](https://api.codeclimate.com/v1/badges/a99a88d28ad37a79dbf6/maintainability)](https://codeclimate.com/github/DenisZhukovski/WebRequest.Elegant)
</h3>

# WebRequest.Elegant
The main idea is to wrap HttpClient type with more elegant and object oriented entity. The entity provides immutable objects by cloning itself and able to make requests to different end points.

```cs
var server = new WebRequest("http://some.server.com"); // application create the WebRequest only once and the reuse it.
var users = server.WithRelativePath("/users"); // new WebRequest object will be created and 
                                               // refers to http://some.server.com/users
var usersResponse = await users.GetResponseAsync();
```

