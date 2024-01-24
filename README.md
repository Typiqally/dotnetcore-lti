# .NET Core LTI
The .NET Core LTI library aims to streamline the process of implementing LTI into your ASP.NET Core projects. It has support for Web API's or fully fledged Web Application with Razor Pages. Configure your application as tool provider, or simply use the identity tokens for authorative purposes.

## Features and roadmap
- LTI tool provider controller, for handling LTI launches
- LTI ID token validator, compatible with [ISecurityTokenValidator](https://learn.microsoft.com/en-us/dotnet/api/microsoft.identitymodel.tokens.isecuritytokenvalidator?view=msal-web-dotnet-latest) from Microsoft Identity Model
- Tool platform storage

## Getting started

### Prerequisites

- [.NET 6](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)

### Installation

The core project can be installed into your project using the following command:

```sh
dotnet add package Tpcly.Lti --version 1.0.0-alpha.4
```

This contains all the basic tools to get up and running with LTI, such as message deserialization and token validation.

## Usage

To extend your project with the LTI services, add the following to your `Program.cs` during the service collection initialization.

```csharp
builder.Services.AddLti()
    .AddPlatforms(new[]
    {
        new ToolPlatform
        {
            Id = "<id>:canvas-lms",
            Name = "",
            Issuer = "https://canvas.instructure.com",
            ClientId = "10000000000001",
            ClientSecret = "<client_secret>",
            AccessTokenUrl = new Uri("http://canvas.docker/login/oauth2/token"),
            AuthorizeUrl = new Uri("http://canvas.docker/api/lti/authorize_redirect"),
            JwkSetUrl = new Uri("http://canvas.docker/api/lti/security/jwks"),
            KeyId = "2018-05-18T22:33:20Z",
        },
    });
```

It is possible to add additional tool platforms from within the `AddPlatforms` method.

### Canvas

If you need specific support for the Canvas LMS, make sure to install the Canvas project using the following command:

```sh
dotnet add package Tpcly.Lti.Canvas --version 1.0.0-alpha.4
```

This project contains handy constants used specifically within the Canvas LMS.

### Tool Provider

If you want to setup an LTI tool provider using solely .NET with MVC and/or Razor Pages, install the tool provider package using the following command:

```
dotnet add package Tpcly.Lti.ToolProvider --version 1.0.0-alpha.4
```

Afterwhich you can add call the `AddToolProvider` method on the LTI builder like so:

```csharp
builder.Services.AddLti()
    .AddToolProvider(options =>
    {
        options.RedirectUri = "/lti/oidc/callback";
        options.Jwk = new JsonWebKey(config["Lti:Jwk"]);
    })
    //.AddPlatforms(...);
```

### Entity Framework

If you wish to store all LTI data using Entity Framework, make sure to install the Entity Framework project using the following command:

```sh
dotnet add package Tpcly.Lti.EntityFramework --version 1.0.0-alpha.4
```

Now it is possible to call the `AddEntityFrameworkRepositories` on the LTI builder like so:

```csharp
builder.Services.AddLti()
    .AddEntityFrameworkRepositories<ApplicationDbContext>()
    //.AddPlatforms(...);
```

Make sure that your `DbContext` extends the `ILtiDbContext` interface, which adds the necessary tables to your existing `DbContext`.

## Meet our contributors

<a href = "https://github.com/Typiqally/dotnetcore-lti/graphs/contributors">
  <img src = "https://contrib.rocks/image?repo=Typiqally/dotnetcore-lti"/>
</a>

## License

.NET Core LTI is licensed under the terms of GPL v3. See [LICENSE](LICENSE) for details.
