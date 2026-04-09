# geoserver.net Usage Guide

This guide shows practical ways to use `geoserver.net` in real projects.

## Install

```bash
dotnet add package geoserver.net
```

## Create a Client (username/password)

```csharp
using geoserver.net;

var options = new GeoServerClientOptions
{
    BaseUri = new Uri("http://localhost:8080/geoserver/rest/"),
    Username = "admin",
    Password = "geoserver"
};

using var client = new GeoServerClient(options);
```

## Reuse Your Own HttpClient

```csharp
using geoserver.net;

var httpClient = new HttpClient();
var options = new GeoServerClientOptions
{
    BaseUri = new Uri("https://example.com/geoserver/rest/"),
    Username = "admin",
    Password = "geoserver",
    Timeout = TimeSpan.FromSeconds(120)
};

// Applies BaseUri + Basic auth + timeout for you:
using var client = httpClient.CreateGeoServerClient(options);
```

## DI Registration (ASP.NET Core)

```csharp
using geoserver.net.DependencyInjection;

services.AddGeoServerClient(options =>
{
    options.BaseUri = new Uri("https://example.com/geoserver/rest/");
    options.Username = "admin";
    options.Password = "geoserver";
});
```

## Workspace CRUD

```csharp
using geoserver.net.Models.Workspaces;

await client.Workspaces.CreateAsync(new WorkspaceCreateRequest { Name = "demo" });
var all = await client.Workspaces.GetAllAsync();
var one = await client.Workspaces.GetByNameAsync("demo");
await client.Workspaces.DeleteAsync("demo", recurse: false);
```

## Typed API Examples

### About

```csharp
var manifest = await client.About.GetManifestTypedAsync();
foreach (var resource in manifest.About.Resources)
{
    Console.WriteLine($"{resource.Name} {resource.Version}");
}
```

### Importer (extension)

```csharp
var imports = await client.Importer.GetAllTypedAsync();
if (imports.Imports.Count > 0 && !string.IsNullOrWhiteSpace(imports.Imports[0].Id))
{
    var detail = await client.Importer.GetByIdTypedAsync(imports.Imports[0].Id!);
    Console.WriteLine(detail.Import.State);
}
```

### GeoWebCache (extension)

```csharp
var gridSets = await client.GeoWebCache.GetGridSetsTypedAsync();
if (gridSets.GridSets.Names.Count > 0)
{
    var gridSet = await client.GeoWebCache.GetGridSetTypedAsync(gridSets.GridSets.Names[0]);
    Console.WriteLine(gridSet.GridSets.Name);
}
```

### Monitoring

```csharp
var list = await client.Operations.GetMonitoringRequestsTypedAsync("list=0&max=1");
if (list.Requests.Count > 0 && !string.IsNullOrWhiteSpace(list.Requests[0].Id))
{
    var request = await client.Operations.GetMonitoringRequestTypedAsync(list.Requests[0].Id!);
    Console.WriteLine($"{request.Request.Method} {request.Request.Path}");
}
```

## Sync API Example

```csharp
var logging = client.Operations.GetLogging();
client.Operations.Reload();
```

## Error Handling

```csharp
try
{
    var layer = await client.Layers.GetByNameAsync("ws:roads");
}
catch (GeoServerApiException ex)
{
    Console.WriteLine($"HTTP {(int)ex.StatusCode}: {ex.ResponseBody}");
    throw;
}
```

## Notes

- Some endpoints require optional GeoServer extensions (for example Importer and GeoWebCache).
- Async and sync methods are available for implemented operations.
- See `README.md` for endpoint coverage and `IMPLEMENTATION_STATUS.md` for detailed status.
- For local integration testing, start GeoServer with `docker compose up -d geoserver` before running `dotnet test` on the integration project.
- You can also orchestrate GeoServer with Aspire using `dotnet run --project src\GeoServer.Net.AppHost\GeoServer.Net.AppHost.csproj`.
