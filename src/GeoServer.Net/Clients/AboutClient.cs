using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using geoserver.net.Models.About;

namespace geoserver.net.Clients;

/// <summary>
/// Wrapper around GeoServer about/manifests/version/status endpoints.
/// </summary>
public sealed class AboutClient : GeoServerClientBase
{
    internal AboutClient(HttpClient httpClient) : base(httpClient)
    {
    }

    /// <summary>
    /// Gets all manifest entries.
    /// </summary>
    public Task<AboutResponse> GetManifestAsync(string? query = null, CancellationToken cancellationToken = default)
        => SendAsync<AboutResponse>(HttpMethod.Get, BuildPath("about/manifest.json", query), cancellationToken: cancellationToken);

    /// <summary>
    /// Gets all manifest entries (synchronous).
    /// </summary>
    public AboutResponse GetManifest(string? query = null)
        => Send<AboutResponse>(HttpMethod.Get, BuildPath("about/manifest.json", query));

    /// <summary>
    /// Gets all manifest entries as typed resources.
    /// </summary>
    public Task<AboutTypedResponse> GetManifestTypedAsync(string? query = null, CancellationToken cancellationToken = default)
        => SendAsync<AboutTypedResponse>(HttpMethod.Get, BuildPath("about/manifest.json", query), cancellationToken: cancellationToken);

    /// <summary>
    /// Gets all manifest entries as typed resources (synchronous).
    /// </summary>
    public AboutTypedResponse GetManifestTyped(string? query = null)
        => Send<AboutTypedResponse>(HttpMethod.Get, BuildPath("about/manifest.json", query));

    /// <summary>
    /// Gets high-level version information for GeoServer/GeoTools/GeoWebCache.
    /// </summary>
    public Task<AboutResponse> GetVersionAsync(string? query = null, CancellationToken cancellationToken = default)
        => SendAsync<AboutResponse>(HttpMethod.Get, BuildPath("about/version.json", query), cancellationToken: cancellationToken);

    /// <summary>
    /// Gets high-level version information for GeoServer/GeoTools/GeoWebCache (synchronous).
    /// </summary>
    public AboutResponse GetVersion(string? query = null)
        => Send<AboutResponse>(HttpMethod.Get, BuildPath("about/version.json", query));

    /// <summary>
    /// Gets high-level version information as typed resources.
    /// </summary>
    public Task<AboutTypedResponse> GetVersionTypedAsync(string? query = null, CancellationToken cancellationToken = default)
        => SendAsync<AboutTypedResponse>(HttpMethod.Get, BuildPath("about/version.json", query), cancellationToken: cancellationToken);

    /// <summary>
    /// Gets high-level version information as typed resources (synchronous).
    /// </summary>
    public AboutTypedResponse GetVersionTyped(string? query = null)
        => Send<AboutTypedResponse>(HttpMethod.Get, BuildPath("about/version.json", query));

    /// <summary>
    /// Gets status information for installed/configured modules.
    /// </summary>
    public Task<AboutResponse> GetStatusAsync(CancellationToken cancellationToken = default)
        => SendAsync<AboutResponse>(HttpMethod.Get, "about/status.json", cancellationToken: cancellationToken);

    /// <summary>
    /// Gets status information for installed/configured modules (synchronous).
    /// </summary>
    public AboutResponse GetStatus()
        => Send<AboutResponse>(HttpMethod.Get, "about/status.json");

    /// <summary>
    /// Gets status information for installed/configured modules as typed resources.
    /// </summary>
    public Task<AboutTypedResponse> GetStatusTypedAsync(CancellationToken cancellationToken = default)
        => SendAsync<AboutTypedResponse>(HttpMethod.Get, "about/status.json", cancellationToken: cancellationToken);

    /// <summary>
    /// Gets status information for installed/configured modules as typed resources (synchronous).
    /// </summary>
    public AboutTypedResponse GetStatusTyped()
        => Send<AboutTypedResponse>(HttpMethod.Get, "about/status.json");

    /// <summary>
    /// Gets system-level status metrics.
    /// </summary>
    public Task<SystemStatusResponse> GetSystemStatusAsync(CancellationToken cancellationToken = default)
        => SendAsync<SystemStatusResponse>(HttpMethod.Get, "about/system-status.json", cancellationToken: cancellationToken);

    /// <summary>
    /// Gets system-level status metrics (synchronous).
    /// </summary>
    public SystemStatusResponse GetSystemStatus()
        => Send<SystemStatusResponse>(HttpMethod.Get, "about/system-status.json");

    private static string BuildPath(string path, string? query)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return path;
        }

        return $"{path}?{query.TrimStart('?')}";
    }
}
