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
}
