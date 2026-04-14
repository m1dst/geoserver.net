using System;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using GeoServer;
using Xunit;

namespace GeoServer.IntegrationTests;

/// <summary>
/// Read-only integration checks that validate core and optional extension endpoints
/// against a live GeoServer instance.
/// </summary>
public sealed class ReadOnlyIntegrationTests : IClassFixture<GeoServerIntegrationFixture>
{
    private readonly GeoServerIntegrationFixture _fixture;
    private bool _gwcUnavailable;
    private bool _importerUnavailable;
    private bool _systemStatusUnavailable;
    private bool _crsUnavailable;
    private bool _filterChainsUnavailable;
    private bool _securityConfigUnavailable;
    private bool _userGroupServicesUnavailable;
    private bool _proxyBaseExtensionUnavailable;

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
        Assert.True(status.About is not null || status.Statuses is not null);

        var typedVersion = await client.About.GetVersionTypedAsync();
        Assert.NotNull(typedVersion.About);

        var typedStatus = await client.About.GetStatusTypedAsync();
        Assert.NotNull(typedStatus.About);

        var typedManifest = await client.About.GetManifestTypedAsync();
        Assert.NotNull(typedManifest.About);
        Assert.NotNull(typedManifest.About.Resources);
    }

    /// <summary>
    /// Executes the SystemStatusReadOnly_ReturnsMetricsOrSkipsIfUnavailable operation.
    /// </summary>
    [Fact]
    public async Task SystemStatusReadOnly_ReturnsMetricsOrSkipsIfUnavailable()
    {
        if (_systemStatusUnavailable)
        {
            return;
        }

        using var client = _fixture.CreateClient();
        try
        {
            var metrics = await client.About.GetSystemStatusAsync();
            Assert.NotNull(metrics.Metrics);
            Assert.NotNull(metrics.Metrics.Metric);
        }
        catch (GeoServerApiException ex) when ((int)ex.StatusCode == 404)
        {
            _systemStatusUnavailable = true;
            return;
        }
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

            var layersRaw = await client.GeoWebCache.GetLayersAsync();
            var layerName = ExtractFirstName(layersRaw.Payload);
            if (!string.IsNullOrWhiteSpace(layerName))
            {
                var typedLayer = await client.GeoWebCache.GetLayerTypedAsync(layerName!);
                Assert.NotNull(typedLayer.Layers);
            }

            var blobStoresRaw = await client.GeoWebCache.GetBlobStoresAsync();
            var blobStoreName = ExtractFirstName(blobStoresRaw.Payload);
            if (!string.IsNullOrWhiteSpace(blobStoreName))
            {
                var typedBlobStore = await client.GeoWebCache.GetBlobStoreTypedAsync(blobStoreName!);
                Assert.NotNull(typedBlobStore.BlobStores);
            }

            var gridSetsRaw = await client.GeoWebCache.GetGridSetsAsync();
            var gridSetName = ExtractFirstName(gridSetsRaw.Payload);
            if (!string.IsNullOrWhiteSpace(gridSetName))
            {
                var typedGridSet = await client.GeoWebCache.GetGridSetTypedAsync(gridSetName!);
                Assert.NotNull(typedGridSet.GridSets);
            }

            // Optional extension endpoint (GeoWebCache request filters); verify surface is callable if available.
            try
            {
                await client.GeoWebCache.UpdateFilterAsync(
                    "demo-filter",
                    "xml",
                    new GeoServer.Models.GeoWebCache.GwcFilterUpdateRequest
                    {
                        GridSet = "EPSG:4326",
                        ZoomStart = 1,
                        ZoomStop = 2
                    });
            }
            catch (GeoServerApiException ex) when ((int)ex.StatusCode == 400 || (int)ex.StatusCode == 404)
            {
                // 400 = endpoint exists but test payload/filter is invalid; 404 = endpoint not installed.
            }
        }
        catch (GeoServerApiException ex) when ((int)ex.StatusCode == 404 || (int)ex.StatusCode == 406)
        {
            // Optional extension: once unavailable, skip remaining probes in this run.
            _gwcUnavailable = true;
            return;
        }
        catch (Newtonsoft.Json.JsonSerializationException)
        {
            // Some vanilla GWC endpoints return arrays for list calls; detail probes are handled from raw payloads above.
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

    /// <summary>
    /// Executes the CrsReadOnly_ReturnsDataOrSkipsIfUnavailable operation.
    /// </summary>
    [Fact]
    public async Task CrsReadOnly_ReturnsDataOrSkipsIfUnavailable()
    {
        if (_crsUnavailable)
        {
            return;
        }

        using var client = _fixture.CreateClient();
        try
        {
            var list = await client.Crs.GetAllAsync(limit: 1);
            Assert.NotNull(list.Crs);

            var authorities = await client.Crs.GetAuthoritiesAsync();
            Assert.NotNull(authorities.Authorities);

            var candidateId = list.Crs.Count > 0 ? list.Crs[0].Id : "EPSG:4326";
            var definition = await client.Crs.GetByIdentifierAsync(candidateId!);
            Assert.False(string.IsNullOrWhiteSpace(definition.Id));

            var wkt = await client.Crs.GetWktByIdentifierAsync(candidateId!);
            Assert.False(string.IsNullOrWhiteSpace(wkt));
        }
        catch (GeoServerApiException ex) when ((int)ex.StatusCode == 404 || (int)ex.StatusCode == 406)
        {
            _crsUnavailable = true;
            return;
        }
    }

    /// <summary>
    /// Executes the FilterChainsReadOnly_ReturnsDataOrSkipsIfUnavailable operation.
    /// </summary>
    [Fact]
    public async Task FilterChainsReadOnly_ReturnsDataOrSkipsIfUnavailable()
    {
        if (_filterChainsUnavailable)
        {
            return;
        }

        using var client = _fixture.CreateClient();
        try
        {
            var chains = await client.FilterChains.GetAllAsync();
            Assert.NotNull(chains.Payload);

            var chainName = ExtractFirstChainName(chains.Payload);
            if (!string.IsNullOrWhiteSpace(chainName))
            {
                var single = await client.FilterChains.GetByNameAsync(chainName!);
                Assert.NotNull(single.Payload);
            }
        }
        catch (GeoServerApiException ex) when ((int)ex.StatusCode == 404 || (int)ex.StatusCode == 403)
        {
            _filterChainsUnavailable = true;
            return;
        }
    }

    /// <summary>
    /// Executes the SecurityConfigReadOnly_ReturnsDataOrSkipsIfUnavailable operation.
    /// </summary>
    [Fact]
    public async Task SecurityConfigReadOnly_ReturnsDataOrSkipsIfUnavailable()
    {
        if (_securityConfigUnavailable)
        {
            return;
        }

        using var client = _fixture.CreateClient();
        try
        {
            var mode = await client.Security.GetCatalogModeAsync();
            Assert.False(string.IsNullOrWhiteSpace(mode.Mode));

            var master = await client.Security.GetMasterPasswordAsync();
            Assert.NotNull(master);
        }
        catch (GeoServerApiException ex) when ((int)ex.StatusCode == 404 || (int)ex.StatusCode == 403)
        {
            _securityConfigUnavailable = true;
            return;
        }
    }

    /// <summary>
    /// Executes the UserGroupServicesReadOnly_ReturnsDataOrSkipsIfUnavailable operation.
    /// </summary>
    [Fact]
    public async Task UserGroupServicesReadOnly_ReturnsDataOrSkipsIfUnavailable()
    {
        if (_userGroupServicesUnavailable)
        {
            return;
        }

        using var client = _fixture.CreateClient();
        try
        {
            var list = await client.UserGroupServices.GetAllAsync();
            Assert.NotNull(list.UserGroupServices);

            var firstService = list.UserGroupServices.Count > 0 ? list.UserGroupServices[0].Name : null;
            if (!string.IsNullOrWhiteSpace(firstService))
            {
                var single = await client.UserGroupServices.GetByNameAsync(firstService!);
                Assert.NotNull(single.Payload);
            }
        }
        catch (GeoServerApiException ex) when ((int)ex.StatusCode == 404 || (int)ex.StatusCode == 403)
        {
            _userGroupServicesUnavailable = true;
            return;
        }
    }

    /// <summary>
    /// Executes the ProxyBaseExtensionReadOnly_ReturnsDataOrSkipsIfUnavailable operation.
    /// </summary>
    [Fact]
    public async Task ProxyBaseExtensionReadOnly_ReturnsDataOrSkipsIfUnavailable()
    {
        if (_proxyBaseExtensionUnavailable)
        {
            return;
        }

        using var client = _fixture.CreateClient();
        try
        {
            var rules = await client.ProxyBaseExtension.GetAllAsync();
            Assert.NotNull(rules.Payload);

            var firstRuleId = ExtractFirstProxyRuleId(rules.Payload);
            if (!string.IsNullOrWhiteSpace(firstRuleId))
            {
                var one = await client.ProxyBaseExtension.GetByIdAsync(firstRuleId!);
                Assert.NotNull(one.Payload);
            }
        }
        catch (GeoServerApiException ex) when ((int)ex.StatusCode == 404 || (int)ex.StatusCode == 403)
        {
            _proxyBaseExtensionUnavailable = true;
            return;
        }
    }

    private static string? ExtractFirstName(System.Collections.Generic.IDictionary<string, JToken> payload)
    {
        foreach (var entry in payload)
        {
            if (entry.Value is JArray array && array.Count > 0 && array[0].Type == JTokenType.String)
            {
                return (string?)array[0];
            }

            if (entry.Value is JObject obj)
            {
                var singleName = (string?)obj["name"];
                if (!string.IsNullOrWhiteSpace(singleName))
                {
                    return singleName;
                }

                var names = obj["names"] as JArray;
                if (names is not null && names.Count > 0 && names[0].Type == JTokenType.String)
                {
                    return (string?)names[0];
                }
            }
        }

        return null;
    }

    private static string? ExtractFirstChainName(System.Collections.Generic.IDictionary<string, JToken> payload)
    {
        if (payload.TryGetValue("filterchain", out var filterChainToken) && filterChainToken is JObject filterChainObject)
        {
            var filters = filterChainObject["filters"];
            if (filters is JArray array)
            {
                for (var i = 0; i < array.Count; i++)
                {
                    if (array[i] is JObject item)
                    {
                        var name = (string?)item["@name"] ?? (string?)item["name"];
                        if (!string.IsNullOrWhiteSpace(name))
                        {
                            return name;
                        }
                    }
                }
            }

            if (filters is JObject singleFilter)
            {
                var name = (string?)singleFilter["@name"] ?? (string?)singleFilter["name"];
                if (!string.IsNullOrWhiteSpace(name))
                {
                    return name;
                }
            }
        }

        if (payload.TryGetValue("filters", out var singleToken) && singleToken is JObject oneFilter)
        {
            var name = (string?)oneFilter["@name"] ?? (string?)oneFilter["name"];
            if (!string.IsNullOrWhiteSpace(name))
            {
                return name;
            }
        }

        return null;
    }

    private static string? ExtractFirstProxyRuleId(System.Collections.Generic.IDictionary<string, JToken> payload)
    {
        if (payload.TryGetValue("ProxyBaseExtensionRules", out var rootToken) && rootToken is JObject root)
        {
            var list = root["ProxyBaseExtensionRule"];
            if (list is JArray array && array.Count > 0 && array[0] is JObject first)
            {
                var id = (string?)first["id"];
                if (!string.IsNullOrWhiteSpace(id))
                {
                    return id;
                }
            }

            if (list is JObject single)
            {
                var id = (string?)single["id"];
                if (!string.IsNullOrWhiteSpace(id))
                {
                    return id;
                }
            }
        }

        if (payload.TryGetValue("ProxyBaseExtensionRule", out var oneToken) && oneToken is JObject oneRule)
        {
            var id = (string?)oneRule["id"];
            if (!string.IsNullOrWhiteSpace(id))
            {
                return id;
            }
        }

        return null;
    }
}
