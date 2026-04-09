using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using geoserver.net.Models.GeoWebCache;

namespace geoserver.net.Clients;

/// <summary>
/// Wrapper around GeoWebCache core endpoints.
/// </summary>
public sealed class GeoWebCacheClient : GeoServerClientBase
{
    internal GeoWebCacheClient(HttpClient httpClient) : base(httpClient)
    {
    }

    /// <summary>
    /// Gets GeoWebCache index HTML.
    /// </summary>
    public Task<string> GetIndexRawAsync(CancellationToken cancellationToken = default)
        => SendRawAsync(HttpMethod.Get, "/geoserver/gwc/rest", cancellationToken: cancellationToken);

    /// <summary>
    /// Gets GeoWebCache index HTML (synchronous).
    /// </summary>
    public string GetIndexRaw()
        => SendRaw(HttpMethod.Get, "/geoserver/gwc/rest");

    /// <summary>
    /// Reloads GeoWebCache settings.
    /// </summary>
    public Task ReloadAsync(object? payload = null, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Post, "/geoserver/gwc/rest/reload", payload, cancellationToken);

    /// <summary>
    /// Reloads GeoWebCache settings (synchronous).
    /// </summary>
    public void Reload(object? payload = null)
        => Send(HttpMethod.Post, "/geoserver/gwc/rest/reload", payload);

    /// <summary>
    /// Gets GeoWebCache global configuration.
    /// </summary>
    public Task<GwcGlobalResponse> GetGlobalAsync(CancellationToken cancellationToken = default)
        => SendAsync<GwcGlobalResponse>(HttpMethod.Get, "/geoserver/gwc/rest/global.json", cancellationToken: cancellationToken);

    /// <summary>
    /// Gets GeoWebCache global configuration (synchronous).
    /// </summary>
    public GwcGlobalResponse GetGlobal()
        => Send<GwcGlobalResponse>(HttpMethod.Get, "/geoserver/gwc/rest/global.json");

    /// <summary>
    /// Gets GeoWebCache global configuration as typed payload.
    /// </summary>
    public Task<GwcGlobalTypedResponse> GetGlobalTypedAsync(CancellationToken cancellationToken = default)
        => SendAsync<GwcGlobalTypedResponse>(HttpMethod.Get, "/geoserver/gwc/rest/global.json", cancellationToken: cancellationToken);

    /// <summary>
    /// Gets GeoWebCache global configuration as typed payload (synchronous).
    /// </summary>
    public GwcGlobalTypedResponse GetGlobalTyped()
        => Send<GwcGlobalTypedResponse>(HttpMethod.Get, "/geoserver/gwc/rest/global.json");

    /// <summary>
    /// Updates GeoWebCache global configuration.
    /// </summary>
    public Task UpdateGlobalAsync(object globalPayload, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Put, "/geoserver/gwc/rest/global", globalPayload, cancellationToken);

    /// <summary>
    /// Updates GeoWebCache global configuration (synchronous).
    /// </summary>
    public void UpdateGlobal(object globalPayload)
        => Send(HttpMethod.Put, "/geoserver/gwc/rest/global", globalPayload);

    /// <summary>
    /// Lists cached layers.
    /// </summary>
    public Task<GwcLayersResponse> GetLayersAsync(CancellationToken cancellationToken = default)
        => SendAsync<GwcLayersResponse>(HttpMethod.Get, "/geoserver/gwc/rest/layers.json", cancellationToken: cancellationToken);

    /// <summary>
    /// Lists cached layers (synchronous).
    /// </summary>
    public GwcLayersResponse GetLayers()
        => Send<GwcLayersResponse>(HttpMethod.Get, "/geoserver/gwc/rest/layers.json");

    /// <summary>
    /// Lists cached layers as typed payload.
    /// </summary>
    public Task<GwcLayersTypedResponse> GetLayersTypedAsync(CancellationToken cancellationToken = default)
        => SendAsync<GwcLayersTypedResponse>(HttpMethod.Get, "/geoserver/gwc/rest/layers.json", cancellationToken: cancellationToken);

    /// <summary>
    /// Lists cached layers as typed payload (synchronous).
    /// </summary>
    public GwcLayersTypedResponse GetLayersTyped()
        => Send<GwcLayersTypedResponse>(HttpMethod.Get, "/geoserver/gwc/rest/layers.json");

    /// <summary>
    /// Gets one cached layer definition.
    /// </summary>
    public Task<GwcLayersResponse> GetLayerAsync(string layerName, CancellationToken cancellationToken = default)
        => SendAsync<GwcLayersResponse>(HttpMethod.Get, $"/geoserver/gwc/rest/layers/{Encode(layerName)}.json", cancellationToken: cancellationToken);

    /// <summary>
    /// Gets one cached layer definition (synchronous).
    /// </summary>
    public GwcLayersResponse GetLayer(string layerName)
        => Send<GwcLayersResponse>(HttpMethod.Get, $"/geoserver/gwc/rest/layers/{Encode(layerName)}.json");

    /// <summary>
    /// Gets one cached layer definition as typed payload.
    /// </summary>
    public Task<GwcLayersTypedResponse> GetLayerTypedAsync(string layerName, CancellationToken cancellationToken = default)
        => SendAsync<GwcLayersTypedResponse>(HttpMethod.Get, $"/geoserver/gwc/rest/layers/{Encode(layerName)}.json", cancellationToken: cancellationToken);

    /// <summary>
    /// Gets one cached layer definition as typed payload (synchronous).
    /// </summary>
    public GwcLayersTypedResponse GetLayerTyped(string layerName)
        => Send<GwcLayersTypedResponse>(HttpMethod.Get, $"/geoserver/gwc/rest/layers/{Encode(layerName)}.json");

    /// <summary>
    /// Creates or updates a cached layer.
    /// </summary>
    public Task PutLayerAsync(string layerName, object layerPayload, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Put, $"/geoserver/gwc/rest/layers/{Encode(layerName)}", layerPayload, cancellationToken);

    /// <summary>
    /// Creates or updates a cached layer (synchronous).
    /// </summary>
    public void PutLayer(string layerName, object layerPayload)
        => Send(HttpMethod.Put, $"/geoserver/gwc/rest/layers/{Encode(layerName)}", layerPayload);

    /// <summary>
    /// Deletes a cached layer.
    /// </summary>
    public Task DeleteLayerAsync(string layerName, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Delete, $"/geoserver/gwc/rest/layers/{Encode(layerName)}", cancellationToken: cancellationToken);

    /// <summary>
    /// Deletes a cached layer (synchronous).
    /// </summary>
    public void DeleteLayer(string layerName)
        => Send(HttpMethod.Delete, $"/geoserver/gwc/rest/layers/{Encode(layerName)}");

    /// <summary>
    /// Gets statuses for running seeding tasks.
    /// </summary>
    public Task<GwcSeedStatusResponse> GetSeedStatusesAsync(CancellationToken cancellationToken = default)
        => SendAsync<GwcSeedStatusResponse>(HttpMethod.Get, "/geoserver/gwc/rest/seed.json", cancellationToken: cancellationToken);

    /// <summary>
    /// Gets statuses for running seeding tasks (synchronous).
    /// </summary>
    public GwcSeedStatusResponse GetSeedStatuses()
        => Send<GwcSeedStatusResponse>(HttpMethod.Get, "/geoserver/gwc/rest/seed.json");

    /// <summary>
    /// Gets statuses for running seeding tasks as typed payload.
    /// </summary>
    public Task<GwcSeedStatusTypedResponse> GetSeedStatusesTypedAsync(CancellationToken cancellationToken = default)
        => SendAsync<GwcSeedStatusTypedResponse>(HttpMethod.Get, "/geoserver/gwc/rest/seed.json", cancellationToken: cancellationToken);

    /// <summary>
    /// Gets statuses for running seeding tasks as typed payload (synchronous).
    /// </summary>
    public GwcSeedStatusTypedResponse GetSeedStatusesTyped()
        => Send<GwcSeedStatusTypedResponse>(HttpMethod.Get, "/geoserver/gwc/rest/seed.json");

    /// <summary>
    /// Gets running seeding status for a specific layer.
    /// </summary>
    public Task<GwcSeedStatusResponse> GetLayerSeedStatusAsync(string layerName, CancellationToken cancellationToken = default)
        => SendAsync<GwcSeedStatusResponse>(HttpMethod.Get, $"/geoserver/gwc/rest/seed/{Encode(layerName)}.json", cancellationToken: cancellationToken);

    /// <summary>
    /// Gets running seeding status for a specific layer (synchronous).
    /// </summary>
    public GwcSeedStatusResponse GetLayerSeedStatus(string layerName)
        => Send<GwcSeedStatusResponse>(HttpMethod.Get, $"/geoserver/gwc/rest/seed/{Encode(layerName)}.json");

    /// <summary>
    /// Gets running seeding status for a specific layer as typed payload.
    /// </summary>
    public Task<GwcSeedStatusTypedResponse> GetLayerSeedStatusTypedAsync(string layerName, CancellationToken cancellationToken = default)
        => SendAsync<GwcSeedStatusTypedResponse>(HttpMethod.Get, $"/geoserver/gwc/rest/seed/{Encode(layerName)}.json", cancellationToken: cancellationToken);

    /// <summary>
    /// Gets running seeding status for a specific layer as typed payload (synchronous).
    /// </summary>
    public GwcSeedStatusTypedResponse GetLayerSeedStatusTyped(string layerName)
        => Send<GwcSeedStatusTypedResponse>(HttpMethod.Get, $"/geoserver/gwc/rest/seed/{Encode(layerName)}.json");

    /// <summary>
    /// Submits a seed/reseed/truncate request for a layer.
    /// </summary>
    public Task SeedLayerAsync(string layerName, object seedRequest, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Post, $"/geoserver/gwc/rest/seed/{Encode(layerName)}.json", seedRequest, cancellationToken);

    /// <summary>
    /// Submits a seed/reseed/truncate request for a layer (synchronous).
    /// </summary>
    public void SeedLayer(string layerName, object seedRequest)
        => Send(HttpMethod.Post, $"/geoserver/gwc/rest/seed/{Encode(layerName)}.json", seedRequest);

    /// <summary>
    /// Gets mass truncate request capabilities as raw XML.
    /// </summary>
    public Task<string> GetMassTruncateCapabilitiesRawAsync(CancellationToken cancellationToken = default)
        => SendRawAsync(HttpMethod.Get, "/geoserver/gwc/rest/masstruncate", cancellationToken: cancellationToken);

    /// <summary>
    /// Gets mass truncate request capabilities as raw XML (synchronous).
    /// </summary>
    public string GetMassTruncateCapabilitiesRaw()
        => SendRaw(HttpMethod.Get, "/geoserver/gwc/rest/masstruncate");

    /// <summary>
    /// Executes a mass truncate request.
    /// </summary>
    public Task MassTruncateAsync(string requestType, string? layer = null, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Post, BuildMassTruncatePath(requestType, layer), cancellationToken: cancellationToken);

    /// <summary>
    /// Executes a mass truncate request (synchronous).
    /// </summary>
    public void MassTruncate(string requestType, string? layer = null)
        => Send(HttpMethod.Post, BuildMassTruncatePath(requestType, layer));

    /// <summary>
    /// Gets disk quota configuration.
    /// </summary>
    public Task<GwcDiskQuotaResponse> GetDiskQuotaAsync(CancellationToken cancellationToken = default)
        => SendAsync<GwcDiskQuotaResponse>(HttpMethod.Get, "/geoserver/gwc/rest/diskquota.json", cancellationToken: cancellationToken);

    /// <summary>
    /// Gets disk quota configuration (synchronous).
    /// </summary>
    public GwcDiskQuotaResponse GetDiskQuota()
        => Send<GwcDiskQuotaResponse>(HttpMethod.Get, "/geoserver/gwc/rest/diskquota.json");

    /// <summary>
    /// Gets disk quota configuration as typed payload.
    /// </summary>
    public Task<GwcDiskQuotaTypedResponse> GetDiskQuotaTypedAsync(CancellationToken cancellationToken = default)
        => SendAsync<GwcDiskQuotaTypedResponse>(HttpMethod.Get, "/geoserver/gwc/rest/diskquota.json", cancellationToken: cancellationToken);

    /// <summary>
    /// Gets disk quota configuration as typed payload (synchronous).
    /// </summary>
    public GwcDiskQuotaTypedResponse GetDiskQuotaTyped()
        => Send<GwcDiskQuotaTypedResponse>(HttpMethod.Get, "/geoserver/gwc/rest/diskquota.json");

    /// <summary>
    /// Updates disk quota configuration.
    /// </summary>
    public Task UpdateDiskQuotaAsync(object diskQuotaPayload, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Put, "/geoserver/gwc/rest/diskquota", diskQuotaPayload, cancellationToken);

    /// <summary>
    /// Updates disk quota configuration (synchronous).
    /// </summary>
    public void UpdateDiskQuota(object diskQuotaPayload)
        => Send(HttpMethod.Put, "/geoserver/gwc/rest/diskquota", diskQuotaPayload);

    /// <summary>
    /// Lists configured blob stores.
    /// </summary>
    public Task<GwcBlobStoresResponse> GetBlobStoresAsync(CancellationToken cancellationToken = default)
        => SendAsync<GwcBlobStoresResponse>(HttpMethod.Get, "/geoserver/gwc/rest/blobstores.json", cancellationToken: cancellationToken);

    /// <summary>
    /// Lists configured blob stores (synchronous).
    /// </summary>
    public GwcBlobStoresResponse GetBlobStores()
        => Send<GwcBlobStoresResponse>(HttpMethod.Get, "/geoserver/gwc/rest/blobstores.json");

    /// <summary>
    /// Lists configured blob stores as typed payload.
    /// </summary>
    public Task<GwcBlobStoresTypedResponse> GetBlobStoresTypedAsync(CancellationToken cancellationToken = default)
        => SendAsync<GwcBlobStoresTypedResponse>(HttpMethod.Get, "/geoserver/gwc/rest/blobstores.json", cancellationToken: cancellationToken);

    /// <summary>
    /// Lists configured blob stores as typed payload (synchronous).
    /// </summary>
    public GwcBlobStoresTypedResponse GetBlobStoresTyped()
        => Send<GwcBlobStoresTypedResponse>(HttpMethod.Get, "/geoserver/gwc/rest/blobstores.json");

    /// <summary>
    /// Gets one configured blob store.
    /// </summary>
    public Task<GwcBlobStoresResponse> GetBlobStoreAsync(string blobStoreName, CancellationToken cancellationToken = default)
        => SendAsync<GwcBlobStoresResponse>(HttpMethod.Get, $"/geoserver/gwc/rest/blobstores/{Encode(blobStoreName)}.json", cancellationToken: cancellationToken);

    /// <summary>
    /// Gets one configured blob store (synchronous).
    /// </summary>
    public GwcBlobStoresResponse GetBlobStore(string blobStoreName)
        => Send<GwcBlobStoresResponse>(HttpMethod.Get, $"/geoserver/gwc/rest/blobstores/{Encode(blobStoreName)}.json");

    /// <summary>
    /// Gets one configured blob store as typed payload.
    /// </summary>
    public Task<GwcBlobStoresTypedResponse> GetBlobStoreTypedAsync(string blobStoreName, CancellationToken cancellationToken = default)
        => SendAsync<GwcBlobStoresTypedResponse>(HttpMethod.Get, $"/geoserver/gwc/rest/blobstores/{Encode(blobStoreName)}.json", cancellationToken: cancellationToken);

    /// <summary>
    /// Gets one configured blob store as typed payload (synchronous).
    /// </summary>
    public GwcBlobStoresTypedResponse GetBlobStoreTyped(string blobStoreName)
        => Send<GwcBlobStoresTypedResponse>(HttpMethod.Get, $"/geoserver/gwc/rest/blobstores/{Encode(blobStoreName)}.json");

    /// <summary>
    /// Creates or updates a blob store.
    /// </summary>
    public Task PutBlobStoreAsync(string blobStoreName, object blobStorePayload, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Put, $"/geoserver/gwc/rest/blobstores/{Encode(blobStoreName)}", blobStorePayload, cancellationToken);

    /// <summary>
    /// Creates or updates a blob store (synchronous).
    /// </summary>
    public void PutBlobStore(string blobStoreName, object blobStorePayload)
        => Send(HttpMethod.Put, $"/geoserver/gwc/rest/blobstores/{Encode(blobStoreName)}", blobStorePayload);

    /// <summary>
    /// Deletes a configured blob store.
    /// </summary>
    public Task DeleteBlobStoreAsync(string blobStoreName, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Delete, $"/geoserver/gwc/rest/blobstores/{Encode(blobStoreName)}", cancellationToken: cancellationToken);

    /// <summary>
    /// Deletes a configured blob store (synchronous).
    /// </summary>
    public void DeleteBlobStore(string blobStoreName)
        => Send(HttpMethod.Delete, $"/geoserver/gwc/rest/blobstores/{Encode(blobStoreName)}");

    /// <summary>
    /// Lists configured grid sets.
    /// </summary>
    public Task<GwcGridSetsResponse> GetGridSetsAsync(CancellationToken cancellationToken = default)
        => SendAsync<GwcGridSetsResponse>(HttpMethod.Get, "/geoserver/gwc/rest/gridsets.json", cancellationToken: cancellationToken);

    /// <summary>
    /// Lists configured grid sets (synchronous).
    /// </summary>
    public GwcGridSetsResponse GetGridSets()
        => Send<GwcGridSetsResponse>(HttpMethod.Get, "/geoserver/gwc/rest/gridsets.json");

    /// <summary>
    /// Lists configured grid sets as typed payload.
    /// </summary>
    public Task<GwcGridSetsTypedResponse> GetGridSetsTypedAsync(CancellationToken cancellationToken = default)
        => SendAsync<GwcGridSetsTypedResponse>(HttpMethod.Get, "/geoserver/gwc/rest/gridsets.json", cancellationToken: cancellationToken);

    /// <summary>
    /// Lists configured grid sets as typed payload (synchronous).
    /// </summary>
    public GwcGridSetsTypedResponse GetGridSetsTyped()
        => Send<GwcGridSetsTypedResponse>(HttpMethod.Get, "/geoserver/gwc/rest/gridsets.json");

    /// <summary>
    /// Gets one configured grid set.
    /// </summary>
    public Task<GwcGridSetsResponse> GetGridSetAsync(string gridSetName, CancellationToken cancellationToken = default)
        => SendAsync<GwcGridSetsResponse>(HttpMethod.Get, $"/geoserver/gwc/rest/gridsets/{Encode(gridSetName)}.json", cancellationToken: cancellationToken);

    /// <summary>
    /// Gets one configured grid set (synchronous).
    /// </summary>
    public GwcGridSetsResponse GetGridSet(string gridSetName)
        => Send<GwcGridSetsResponse>(HttpMethod.Get, $"/geoserver/gwc/rest/gridsets/{Encode(gridSetName)}.json");

    /// <summary>
    /// Gets one configured grid set as typed payload.
    /// </summary>
    public Task<GwcGridSetsTypedResponse> GetGridSetTypedAsync(string gridSetName, CancellationToken cancellationToken = default)
        => SendAsync<GwcGridSetsTypedResponse>(HttpMethod.Get, $"/geoserver/gwc/rest/gridsets/{Encode(gridSetName)}.json", cancellationToken: cancellationToken);

    /// <summary>
    /// Gets one configured grid set as typed payload (synchronous).
    /// </summary>
    public GwcGridSetsTypedResponse GetGridSetTyped(string gridSetName)
        => Send<GwcGridSetsTypedResponse>(HttpMethod.Get, $"/geoserver/gwc/rest/gridsets/{Encode(gridSetName)}.json");

    /// <summary>
    /// Creates or updates a grid set.
    /// </summary>
    public Task PutGridSetAsync(string gridSetName, object gridSetPayload, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Put, $"/geoserver/gwc/rest/gridsets/{Encode(gridSetName)}", gridSetPayload, cancellationToken);

    /// <summary>
    /// Creates or updates a grid set (synchronous).
    /// </summary>
    public void PutGridSet(string gridSetName, object gridSetPayload)
        => Send(HttpMethod.Put, $"/geoserver/gwc/rest/gridsets/{Encode(gridSetName)}", gridSetPayload);

    /// <summary>
    /// Deletes a configured grid set.
    /// </summary>
    public Task DeleteGridSetAsync(string gridSetName, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Delete, $"/geoserver/gwc/rest/gridsets/{Encode(gridSetName)}", cancellationToken: cancellationToken);

    /// <summary>
    /// Deletes a configured grid set (synchronous).
    /// </summary>
    public void DeleteGridSet(string gridSetName)
        => Send(HttpMethod.Delete, $"/geoserver/gwc/rest/gridsets/{Encode(gridSetName)}");

    /// <summary>
    /// Gets bounds text for a layer/SRS pair.
    /// </summary>
    public Task<string> GetBoundsRawAsync(string layerName, string srs, CancellationToken cancellationToken = default)
        => SendRawAsync(HttpMethod.Get, $"/geoserver/gwc/rest/bounds/{Encode(layerName)}/{Encode(srs)}/java", cancellationToken: cancellationToken);

    /// <summary>
    /// Gets bounds text for a layer/SRS pair (synchronous).
    /// </summary>
    public string GetBoundsRaw(string layerName, string srs)
        => SendRaw(HttpMethod.Get, $"/geoserver/gwc/rest/bounds/{Encode(layerName)}/{Encode(srs)}/java");

    private static string BuildMassTruncatePath(string requestType, string? layer)
    {
        if (string.IsNullOrWhiteSpace(requestType))
        {
            throw new System.ArgumentException("Value is required.", nameof(requestType));
        }

        var path = $"/geoserver/gwc/rest/masstruncate?requestType={System.Uri.EscapeDataString(requestType)}";
        if (!string.IsNullOrWhiteSpace(layer))
        {
            path += $"&layer={System.Uri.EscapeDataString(layer)}";
        }

        return path;
    }

    private static string Encode(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new System.ArgumentException("Value is required.", nameof(value));
        }

        return System.Uri.EscapeDataString(value);
    }
}
