# geoserver.net

`geoserver.net` is a strongly typed GeoServer REST wrapper targeting both `net10.0` and `net48`.

## Features

- Async and synchronous APIs for each implemented endpoint.
- Typed request/response models.
- Centralized HTTP error handling (`GeoServerApiException`).
- Unit tests validating each endpoint method behavior.
- Integration tests against a real GeoServer instance.
- CI support for GitHub Actions.
- Tag-based GitHub Actions release workflow with optional NuGet publish.
- NuGet-ready packaging configuration.
- CI explicitly builds both `net10.0` and `net48` targets.

## Implemented Endpoints

- Workspaces
- Data Stores
- Coverage Stores
- Coverages
- Structured Coverages
- Feature Types
- Namespaces
- Layer Groups
- Layers
- Styles
- Settings (global, contact, workspace)
- OWS Service Settings (WMS/WFS/WCS/WMTS, global + workspace)
- Security Roles
- Security User/Group management
- Security User/Group services configuration
- Security root configuration (master password, self password, catalog mode, ACL catalog reload)
- Security Authentication Providers
- Security Authentication Filters
- Security Filter Chains
- WMS Stores and Layers
- WMTS Stores and Layers
- Operations (Reset, Reload, Logging, Monitoring)
- About endpoints (Manifest, Version, Status, System Status)
- CRS endpoints (list, authorities, definition JSON, definition WKT)
- URL Checks endpoints
- Proxy Base Extension endpoints
- Resource endpoints (read metadata/content, HEAD, upload/update, delete)
- Fonts endpoint
- Templates endpoints (server/workspace/store/type/coverage scopes)
- WFS transforms endpoints
- GeoWebCache core endpoints (index, reload, global config)
- GeoWebCache layer and seed endpoints
- GeoWebCache mass truncate and disk quota endpoints
- GeoWebCache blobstores, gridsets, and bounds endpoints
- GeoWebCache filter update endpoint
- Importer extension core endpoints (imports lifecycle)
- Importer extension task and data endpoints
- Importer extension transform endpoints
- Importer upload helpers (raw file upload + URL task creation)
- Importer multipart/form-data upload helper
- Typed importer response methods for core/tasks/transforms
- Typed about response methods for manifest/version/status
- Typed monitoring response methods for requests list/detail
- Typed GeoWebCache response methods for global/layers/seed/diskquota/blobstores/gridsets
- Typed logging response model with backward-compatible `Logging` alias

See `IMPLEMENTATION_STATUS.md` for implemented operations and remaining API surface.
For a focused setup and code walkthrough, see `USAGE.md`.

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

Use your own `HttpClient` (for example from `IHttpClientFactory`) so connections are pooled and reused.
If you still want to use `GeoServerClientOptions` for auth/base URL/timeout, use the options overload:

```csharp
using geoserver.net;
var options = new GeoServerClientOptions
{
    BaseUri = new Uri("https://example.com/geoserver/rest/"),
    Username = "admin",
    Password = "geoserver"
};

// httpClient comes from IHttpClientFactory / DI
using var geo = httpClient.CreateGeoServerClient(options); // options applied automatically
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

## Typed API Examples

```csharp
using var client = new GeoServerClient(options);

// About (typed)
var about = await client.About.GetManifestTypedAsync();
foreach (var resource in about.About.Resources)
{
    Console.WriteLine($"{resource.Name} ({resource.Version})");
}

// Importer (typed)
var imports = await client.Importer.GetAllTypedAsync();
if (imports.Imports.Count > 0 && !string.IsNullOrWhiteSpace(imports.Imports[0].Id))
{
    var importDetail = await client.Importer.GetByIdTypedAsync(imports.Imports[0].Id!);
    Console.WriteLine(importDetail.Import.State);
}

// GeoWebCache (typed)
var gridSets = await client.GeoWebCache.GetGridSetsTypedAsync();
if (gridSets.GridSets.Names.Count > 0)
{
    var gridSet = await client.GeoWebCache.GetGridSetTypedAsync(gridSets.GridSets.Names[0]);
    Console.WriteLine(gridSet.GridSets.Name);
}

// Monitoring (typed)
var requests = await client.Operations.GetMonitoringRequestsTypedAsync("list=0&max=1");
if (requests.Requests.Count > 0 && !string.IsNullOrWhiteSpace(requests.Requests[0].Id))
{
    var request = await client.Operations.GetMonitoringRequestTypedAsync(requests.Requests[0].Id!);
    Console.WriteLine($"{request.Request.Method} {request.Request.Path}");
}
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

One-command helper (start, wait, test, shutdown):

```bash
powershell -ExecutionPolicy Bypass -File .\scripts\run-integration.ps1
powershell -ExecutionPolicy Bypass -File .\scripts\run-integration.ps1 -DetailedTestOutput
```

The helper is location-independent, so you can run it from any current directory.

Integration tests include:
- Workspace CRUD flow.
- Read-only About endpoint validation.
- GeoWebCache read-only checks (auto-skips if GWC is unavailable).
- Importer read-only checks (auto-skips if Importer extension is unavailable).
- Typed read-only checks for About (manifest/version/status), GeoWebCache (list/detail), Importer (list/detail), plus monitoring typed list/detail probes.

## GitHub Actions Release Publish

- CI validation runs on push and pull request via `.github/workflows/build-test-pack.yml`.
- Release publish runs on `v*` tags (and manual dispatch) via `.github/workflows/release-publish.yml`.
- To publish packages to NuGet, set repository secret `NUGET_API_KEY`.
- Release package version is computed by Nerdbank.GitVersioning from `version.json` + git history.
- Tag names trigger release jobs, but do not directly set NuGet package version.

Release checklist:

- Ensure `version.json` has the desired base version (for example `1.0-beta` for prerelease or `1.0` for stable).
- Check computed package version locally before tagging: `dotnet tool update -g nbgv || dotnet tool install -g nbgv` then `nbgv get-version -v NuGetPackageVersion`.
- Push a `v*` tag to trigger release workflow.
- In workflow logs, confirm `Show computed NuGet package version (NBGV)` output before publish.

## Running GeoServer Locally with Docker

```bash
docker compose up -d geoserver
```

Health and endpoint check:

```bash
curl -u admin:geoserver http://localhost:8080/geoserver/rest/about/status.json
```

## Running GeoServer with Aspire (optional)

```bash
dotnet run --project src\GeoServer.Net.AppHost\GeoServer.Net.AppHost.csproj
```

Then run integration tests in a separate shell:

```bash
set GEOSERVER_BASE_URL=http://localhost:8080/geoserver/rest/
set GEOSERVER_USERNAME=admin
set GEOSERVER_PASSWORD=geoserver
dotnet test tests\GeoServer.Net.IntegrationTests\GeoServer.Net.IntegrationTests.csproj
```

## Packaging

```bash
dotnet pack src\GeoServer.Net\GeoServer.Net.csproj -c Release -o .\artifacts\packages
```

## Versioning

This repository uses Nerdbank.GitVersioning (NBGV) to derive package and assembly versions from git history.

- Version configuration lives in `version.json` at repo root.
- Base version is currently `1.0-beta`.
- Only tags matching `vX.Y` or `vX.Y.Z` are treated as public releases.
- CI uses full clone history so NBGV can compute version metadata correctly.

Typical outcomes with current `version.json` (`1.0-beta`):

- Normal branch build: `1.0.<height>-beta`
- Tagged release build: still `1.0.<height>-beta` unless `version.json` is changed to a stable base.

Local checks:

```bash
dotnet build src\GeoServer.Net\GeoServer.Net.csproj -c Release
dotnet pack src\GeoServer.Net\GeoServer.Net.csproj -c Release -o .\artifacts\packages
```

Create a stable release tag:

```bash
git tag v1.0.0
git push origin v1.0.0
```

## Notes

- GeoServer REST documentation: <https://docs.geoserver.org/main/en/user/rest/>
- This project follows phased endpoint expansion; phase progress is tracked in `IMPLEMENTATION_STATUS.md`.
- XML documentation comments are included across public API surface and typed DTOs; targeted inline comments are used in complex test/control-flow sections.

