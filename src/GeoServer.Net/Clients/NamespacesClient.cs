using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using geoserver.net.Models.Namespaces;

namespace geoserver.net.Clients;

/// <summary>
/// Wrapper around GeoServer namespaces REST endpoints.
/// </summary>
public sealed class NamespacesClient : GeoServerClientBase
{
    internal NamespacesClient(HttpClient httpClient) : base(httpClient)
    {
    }

    /// <summary>
    /// Gets all namespaces.
    /// </summary>
    public Task<NamespaceListResponse> GetAllAsync(CancellationToken cancellationToken = default)
        => SendAsync<NamespaceListResponse>(HttpMethod.Get, "namespaces.json", cancellationToken: cancellationToken);

    /// <summary>
    /// Gets all namespaces (synchronous).
    /// </summary>
    public NamespaceListResponse GetAll() => Send<NamespaceListResponse>(HttpMethod.Get, "namespaces.json");

    /// <summary>
    /// Gets one namespace by prefix.
    /// </summary>
    public Task<NamespaceResponse> GetByPrefixAsync(string prefix, CancellationToken cancellationToken = default)
        => SendAsync<NamespaceResponse>(HttpMethod.Get, $"namespaces/{Encode(prefix)}.json", cancellationToken: cancellationToken);

    /// <summary>
    /// Gets one namespace by prefix (synchronous).
    /// </summary>
    public NamespaceResponse GetByPrefix(string prefix)
        => Send<NamespaceResponse>(HttpMethod.Get, $"namespaces/{Encode(prefix)}.json");

    /// <summary>
    /// Creates a namespace.
    /// </summary>
    public Task CreateAsync(NamespaceCreateRequest request, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Post, "namespaces", new { @namespace = request }, cancellationToken);

    /// <summary>
    /// Creates a namespace (synchronous).
    /// </summary>
    public void Create(NamespaceCreateRequest request)
        => Send(HttpMethod.Post, "namespaces", new { @namespace = request });

    /// <summary>
    /// Updates a namespace.
    /// </summary>
    public Task UpdateAsync(string prefix, NamespaceCreateRequest request, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Put, $"namespaces/{Encode(prefix)}", new { @namespace = request }, cancellationToken);

    /// <summary>
    /// Updates a namespace (synchronous).
    /// </summary>
    public void Update(string prefix, NamespaceCreateRequest request)
        => Send(HttpMethod.Put, $"namespaces/{Encode(prefix)}", new { @namespace = request });

    /// <summary>
    /// Deletes a namespace.
    /// </summary>
    public Task DeleteAsync(string prefix, bool recurse = false, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Delete, $"namespaces/{Encode(prefix)}?recurse={recurse.ToString().ToLowerInvariant()}", cancellationToken: cancellationToken);

    /// <summary>
    /// Deletes a namespace (synchronous).
    /// </summary>
    public void Delete(string prefix, bool recurse = false)
        => Send(HttpMethod.Delete, $"namespaces/{Encode(prefix)}?recurse={recurse.ToString().ToLowerInvariant()}");

    private static string Encode(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Value is required.", nameof(value));
        }

        return Uri.EscapeDataString(value);
    }
}
