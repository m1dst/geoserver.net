# geoserver.net

`geoserver.net` is a strongly typed GeoServer REST wrapper targeting both `net10.0` and `net48`.

## Features

- Async and synchronous APIs for each implemented endpoint.
- Typed request/response models.
- Centralized HTTP error handling (`GeoServerApiException`).
- Unit tests validating each endpoint method behavior.
- Integration tests against a real GeoServer instance.
- CI support for GitHub Actions and Bitbucket Pipelines.
- NuGet-ready packaging configuration.

## Implemented Endpoints (Phase 1)

- Workspaces
- Data Stores
- Coverage Stores
- Feature Types
- Layers
- Styles

See `IMPLEMENTATION_STATUS.md` for implemented operations and remaining API surface.

## Install

```bash
dotnet add package geoserver.net
```

## Quick Start

```csharp
using geoserver.net;
using geoserver.net.Models.Workspaces;

var options = new GeoServerClientOptions
{
    BaseUri = new Uri("http://localhost:8080/geoserver/rest/"),
    Username = "admin",
    Password = "geoserver"
};

using var client = new GeoServerClient(options);

await client.Workspaces.CreateAsync(new WorkspaceCreateRequest { Name = "demo" });
var workspaces = await client.Workspaces.GetAllAsync();

// Sync equivalents are available for every operation:
client.Workspaces.Delete("demo");
```

## Reusing Your Own HttpClient (ASP.NET / DI)

Use your own `HttpClient` (for example from `IHttpClientFactory`) so connections are pooled and reused:

```csharp
using geoserver.net;

// httpClient comes from IHttpClientFactory / DI
httpClient.BaseAddress = new Uri("https://example.com/geoserver/rest/");
httpClient.DefaultRequestHeaders.Authorization =
    new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", "<base64-user-pass>");

using var geo = new GeoServerClient(httpClient); // does NOT dispose external HttpClient by default
var layers = await geo.Layers.GetAllAsync();
```

Or use the helper extension:

```csharp
using var geo = httpClient.CreateGeoServerClient();
```

Or register it in ASP.NET Core DI in one line:

```csharp
services.AddGeoServerClient(options =>
{
    options.BaseUri = new Uri("https://example.com/geoserver/rest/");
    options.Username = "admin";
    options.Password = "geoserver";
});
```

## Running Tests

Unit tests:

```bash
dotnet test tests\GeoServer.Net.Tests\GeoServer.Net.Tests.csproj
```

Integration tests (requires running GeoServer):

```bash
set GEOSERVER_BASE_URL=http://localhost:8080/geoserver/rest/
set GEOSERVER_USERNAME=admin
set GEOSERVER_PASSWORD=geoserver
dotnet test tests\GeoServer.Net.IntegrationTests\GeoServer.Net.IntegrationTests.csproj
```

## Running GeoServer Locally with Docker

```bash
docker compose up -d geoserver
```

## Packaging

```bash
dotnet pack src\GeoServer.Net\GeoServer.Net.csproj -c Release -o .\artifacts\packages
```

## Notes

- GeoServer REST documentation: <https://docs.geoserver.org/main/en/user/rest/>
- This project follows phased endpoint expansion; phase progress is tracked in `IMPLEMENTATION_STATUS.md`.
