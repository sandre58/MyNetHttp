<div id="top"></div>

<!-- PROJECT INFO -->
<br />
<div align="center">
  <a href="https://github.com/sandre58/MyNetHttp">
    <img src="images/logo.png" width="256" height="256">
  </a>

<h1 align="center">My .NET Http</h1>

[![Downloads][downloads-shield]][downloads-url]
[![Forks][forks-shield]][forks-url]
[![Stargazers][stars-shield]][stars-url]
[![Issues][issues-shield]][issues-url]
[![MIT License][license-shield]][license-url]

  <p align="center">
    <br />
    The .NET Http is a versatile class library designed to simplify the process of converting objects into human-readable strings in .NET applications. This library provides developers with an easy-to-use interface for generating informative and understandable string representations of complex objects.
    <br />
    Supporting only .NET 8.0
  </p>

[![Language][language-shield]][language-url]
[![Framework][framework-shield]][framework-url]
[![Version][version-shield]][version-url]
[![Build][build-shield]][build-url]

</div>

## Getting Started

To start using My .NET Http in your project, follow these steps:

1. Install the library via NuGet Package Manager:
   ```bash
   dotnet add package MyNet.Http

## What's included ?

### Simple HTTP Requests

- **HTTP Method Support**: Send HTTP requests using common methods such as GET, POST, PUT, DELETE, and more.

- **Request Configuration**: Configure request headers, query parameters, request body, and authentication credentials easily using intuitive methods and parameters.

- **Response Handling**: Process HTTP responses and extract response data, status codes, headers, and content efficiently for further processing.

### Flexibility and Customization

- **HTTP Client Configuration**: Customize HTTP client settings such as timeout, connection pooling, and proxy configuration to suit specific application requirements.

- **Middleware Integration**: Seamlessly integrate with ASP.NET Core middleware pipeline or other middleware frameworks for advanced request processing and interception.

### Asynchronous Support

- **Async/Await Pattern**: Utilize asynchronous programming patterns to send and handle HTTP requests asynchronously, ensuring responsive and efficient application performance.

- **Cancellation and Timeout Handling**: Implement cancellation and timeout mechanisms to prevent long-running requests from blocking the application thread pool.

## License

Copyright © Stéphane ANDRE.

My .NET Http is provided as-is under the MIT license. For more information see [LICENSE](./LICENSE).

<!-- MARKDOWN LINKS & IMAGES -->
<!-- https://www.markdownguide.org/basic-syntax/#reference-style-links -->
[language-shield]: https://img.shields.io/github/languages/top/sandre58/MyNetHttp
[language-url]: https://github.com/sandre58/MyNetHttp
[forks-shield]: https://img.shields.io/github/forks/sandre58/MyNetHttp?style=for-the-badge
[forks-url]: https://github.com/sandre58/MyNetHttp/network/members
[stars-shield]: https://img.shields.io/github/stars/sandre58/MyNetHttp?style=for-the-badge
[stars-url]: https://github.com/sandre58/MyNetHttp/stargazers
[issues-shield]: https://img.shields.io/github/issues/sandre58/MyNetHttp?style=for-the-badge
[issues-url]: https://github.com/sandre58/MyNetHttp/issues
[license-shield]: https://img.shields.io/github/license/sandre58/MyNetHttp?style=for-the-badge
[license-url]: https://github.com/sandre58/MyNetHttp/blob/main/LICENSE
[build-shield]: https://img.shields.io/github/actions/workflow/status/sandre58/MyNetHttp/ci.yml?logo=github&label=CI
[build-url]: https://github.com/sandre58/MyNetHttp/actions
[downloads-shield]: https://img.shields.io/github/downloads/sandre58/MyNetHttp/total?style=for-the-badge
[downloads-url]: https://github.com/sandre58/MyNetHttp/releases
[framework-shield]: https://img.shields.io/badge/.NET-8.0-purple
[framework-url]: https://github.com/sandre58/MyNetHttp/tree/main/src/MyNet.Http
[version-shield]: https://img.shields.io/nuget/v/MyNet.Http
[version-url]: https://www.nuget.org/packages/MyNet.Http