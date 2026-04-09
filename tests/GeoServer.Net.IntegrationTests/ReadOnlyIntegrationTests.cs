using System;
using System.Threading.Tasks;
using geoserver.net;
using Xunit;

namespace GeoServer.Net.IntegrationTests;

/// <summary>
/// Read-only integration checks that validate core and optional extension endpoints
/// against a live GeoServer instance.
/// </summary>
public sealed class ReadOnlyIntegrationTests : IClassFixture<GeoServerIntegrationFixture>
{
    private readonly GeoServerIntegrationFixture _fixture;
    private bool _gwcUnavailable;
    private bool _importerUnavailable;

    public ReadOnlyIntegrationTests(GeoServerIntegrationFixture fixture)
    {
        _fixture = fixture;
    }

    /// <summary>
    /// Executes the AboutEndpoints_ReturnMetadata operation.
    /// </summary>
    [Fact]
    public async Task AboutEndpoints_ReturnMetadata()
    {
        using var client = _fixture.CreateClient();

        var version = await client.About.GetVersionAsync();
        Assert.NotNull(version.About);

        var status = await client.About.GetStatusAsync();
        Assert.NotNull(status.About);

        var typedVersion = await client.About.GetVersionTypedAsync();
        Assert.NotNull(typedVersion.About);

        var typedStatus = await client.About.GetStatusTypedAsync();
        Assert.NotNull(typedStatus.About);

        var typedManifest = await client.About.GetManifestTypedAsync();
        Assert.NotNull(typedManifest.About);
        Assert.NotNull(typedManifest.About.Resources);
    }

    /// <summary>
    /// Executes the GeoWebCacheCoreReadOnly_ReturnsDataOrSkipsIfUnavailable operation.
    /// </summary>
    [Fact]
    public async Task GeoWebCacheCoreReadOnly_ReturnsDataOrSkipsIfUnavailable()
    {
        if (_gwcUnavailable)
        {
            return;
        }

        using var client = _fixture.CreateClient();

        try
        {
            var index = await client.GeoWebCache.GetIndexRawAsync();
            Assert.False(string.IsNullOrWhiteSpace(index));

            var global = await client.GeoWebCache.GetGlobalAsync();
            Assert.NotNull(global.Global);

            var typedGlobal = await client.GeoWebCache.GetGlobalTypedAsync();
            Assert.NotNull(typedGlobal.Global);

            var typedLayers = await client.GeoWebCache.GetLayersTypedAsync();
            Assert.NotNull(typedLayers.Layers);

            // Some servers expose a single "name", others expose a "names" collection.
            var layerName = !string.IsNullOrWhiteSpace(typedLayers.Layers.Name)
                ? typedLayers.Layers.Name
                : typedLayers.Layers.Names.Count > 0 ? typedLayers.Layers.Names[0] : null;
            if (!string.IsNullOrWhiteSpace(layerName))
            {
                var typedLayer = await client.GeoWebCache.GetLayerTypedAsync(layerName!);
                Assert.NotNull(typedLayer.Layers);
            }

            var typedBlobStores = await client.GeoWebCache.GetBlobStoresTypedAsync();
            Assert.NotNull(typedBlobStores.BlobStores);

            // Same normalization pattern for optional detail probes.
            var blobStoreName = !string.IsNullOrWhiteSpace(typedBlobStores.BlobStores.Name)
                ? typedBlobStores.BlobStores.Name
                : typedBlobStores.BlobStores.Names.Count > 0 ? typedBlobStores.BlobStores.Names[0] : null;
            if (!string.IsNullOrWhiteSpace(blobStoreName))
            {
                var typedBlobStore = await client.GeoWebCache.GetBlobStoreTypedAsync(blobStoreName!);
                Assert.NotNull(typedBlobStore.BlobStores);
            }

            var typedGridSets = await client.GeoWebCache.GetGridSetsTypedAsync();
            Assert.NotNull(typedGridSets.GridSets);

            var gridSetName = !string.IsNullOrWhiteSpace(typedGridSets.GridSets.Name)
                ? typedGridSets.GridSets.Name
                : typedGridSets.GridSets.Names.Count > 0 ? typedGridSets.GridSets.Names[0] : null;
            if (!string.IsNullOrWhiteSpace(gridSetName))
            {
                var typedGridSet = await client.GeoWebCache.GetGridSetTypedAsync(gridSetName!);
                Assert.NotNull(typedGridSet.GridSets);
            }
        }
        catch (GeoServerApiException ex) when ((int)ex.StatusCode == 404)
        {
            // Optional extension: once unavailable, skip remaining probes in this run.
            _gwcUnavailable = true;
            return;
        }
    }

    /// <summary>
    /// Executes the ImporterReadOnly_ReturnsDataOrSkipsIfUnavailable operation.
    /// </summary>
    [Fact]
    public async Task ImporterReadOnly_ReturnsDataOrSkipsIfUnavailable()
    {
        if (_importerUnavailable)
        {
            return;
        }

        using var client = _fixture.CreateClient();

        try
        {
            var imports = await client.Importer.GetAllAsync();
            Assert.NotNull(imports.Payload);

            var typedImports = await client.Importer.GetAllTypedAsync();
            Assert.NotNull(typedImports.Imports);

            var firstImportId = typedImports.Imports.Count > 0 ? typedImports.Imports[0].Id : null;
            if (!string.IsNullOrWhiteSpace(firstImportId))
            {
                var typedImport = await client.Importer.GetByIdTypedAsync(firstImportId!);
                Assert.NotNull(typedImport.Import);
            }
        }
        catch (GeoServerApiException ex) when ((int)ex.StatusCode == 404)
        {
            // Optional extension: once unavailable, skip remaining probes in this run.
            _importerUnavailable = true;
            return;
        }
    }

    /// <summary>
    /// Executes the MonitoringTypedReadOnly_ReturnsDataOrSkipsIfUnavailable operation.
    /// </summary>
    [Fact]
    public async Task MonitoringTypedReadOnly_ReturnsDataOrSkipsIfUnavailable()
    {
        using var client = _fixture.CreateClient();

        try
        {
            var requests = await client.Operations.GetMonitoringRequestsTypedAsync("list=0&max=1");
            Assert.NotNull(requests.Requests);

            var firstId = requests.Requests.Count > 0 ? requests.Requests[0].Id : null;
            if (!string.IsNullOrWhiteSpace(firstId))
            {
                var single = await client.Operations.GetMonitoringRequestTypedAsync(firstId!);
                Assert.NotNull(single.Request);
            }
        }
        catch (GeoServerApiException ex) when ((int)ex.StatusCode == 404)
        {
            return;
        }
    }
}
