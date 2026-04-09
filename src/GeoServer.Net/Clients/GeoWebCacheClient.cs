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
    /// Submits a seed/reseed/truncate request for a layer.
    /// </summary>
    public Task SeedLayerAsync(string layerName, object seedRequest, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Post, $"/geoserver/gwc/rest/seed/{Encode(layerName)}.json", seedRequest, cancellationToken);

    /// <summary>
    /// Submits a seed/reseed/truncate request for a layer (synchronous).
    /// </summary>
    public void SeedLayer(string layerName, object seedRequest)
        => Send(HttpMethod.Post, $"/geoserver/gwc/rest/seed/{Encode(layerName)}.json", seedRequest);

    private static string Encode(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new System.ArgumentException("Value is required.", nameof(value));
        }

        return System.Uri.EscapeDataString(value);
    }
}
