using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using geoserver.net.Models.UrlChecks;

namespace geoserver.net.Clients;

/// <summary>
/// Wrapper around URL external access checks endpoints.
/// </summary>
public sealed class UrlChecksClient : GeoServerClientBase
{
    internal UrlChecksClient(HttpClient httpClient) : base(httpClient)
    {
    }

    /// <summary>
    /// Lists all URL checks.
    /// </summary>
    public Task<UrlCheckListResponse> GetAllAsync(CancellationToken cancellationToken = default)
        => SendAsync<UrlCheckListResponse>(HttpMethod.Get, "urlchecks.json", cancellationToken: cancellationToken);

    /// <summary>
    /// Lists all URL checks (synchronous).
    /// </summary>
    public UrlCheckListResponse GetAll()
        => Send<UrlCheckListResponse>(HttpMethod.Get, "urlchecks.json");

    /// <summary>
    /// Gets a URL check by name.
    /// </summary>
    public Task<UrlCheckResponse> GetByNameAsync(string urlCheckName, CancellationToken cancellationToken = default)
        => SendAsync<UrlCheckResponse>(HttpMethod.Get, $"urlchecks/{Encode(urlCheckName)}.json", cancellationToken: cancellationToken);

    /// <summary>
    /// Gets a URL check by name (synchronous).
    /// </summary>
    public UrlCheckResponse GetByName(string urlCheckName)
        => Send<UrlCheckResponse>(HttpMethod.Get, $"urlchecks/{Encode(urlCheckName)}.json");

    /// <summary>
    /// Creates a URL check.
    /// </summary>
    public Task CreateAsync(object urlCheckPayload, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Post, "urlchecks", urlCheckPayload, cancellationToken);

    /// <summary>
    /// Creates a URL check (synchronous).
    /// </summary>
    public void Create(object urlCheckPayload)
        => Send(HttpMethod.Post, "urlchecks", urlCheckPayload);

    /// <summary>
    /// Updates a URL check.
    /// </summary>
    public Task UpdateAsync(string urlCheckName, object urlCheckPayload, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Put, $"urlchecks/{Encode(urlCheckName)}", urlCheckPayload, cancellationToken);

    /// <summary>
    /// Updates a URL check (synchronous).
    /// </summary>
    public void Update(string urlCheckName, object urlCheckPayload)
        => Send(HttpMethod.Put, $"urlchecks/{Encode(urlCheckName)}", urlCheckPayload);

    /// <summary>
    /// Deletes a URL check.
    /// </summary>
    public Task DeleteAsync(string urlCheckName, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Delete, $"urlchecks/{Encode(urlCheckName)}", cancellationToken: cancellationToken);

    /// <summary>
    /// Deletes a URL check (synchronous).
    /// </summary>
    public void Delete(string urlCheckName)
        => Send(HttpMethod.Delete, $"urlchecks/{Encode(urlCheckName)}");

    private static string Encode(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Value is required.", nameof(value));
        }

        return Uri.EscapeDataString(value);
    }
}
