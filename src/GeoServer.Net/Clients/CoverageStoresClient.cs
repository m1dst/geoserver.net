using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using geoserver.net.Models.Stores;

namespace geoserver.net.Clients;

/// <summary>
/// Wrapper around GeoServer coverage stores REST endpoints.
/// </summary>
public sealed class CoverageStoresClient : GeoServerClientBase
{
    internal CoverageStoresClient(HttpClient httpClient) : base(httpClient)
    {
    }

    /// <summary>
    /// Gets coverage stores for a workspace.
    /// </summary>
    public Task<CoverageStoreListResponse> GetAllAsync(string workspaceName, CancellationToken cancellationToken = default)
        => SendAsync<CoverageStoreListResponse>(HttpMethod.Get, $"workspaces/{Encode(workspaceName)}/coveragestores.json", cancellationToken: cancellationToken);

    /// <summary>
    /// Gets coverage stores for a workspace (synchronous).
    /// </summary>
    public CoverageStoreListResponse GetAll(string workspaceName)
        => Send<CoverageStoreListResponse>(HttpMethod.Get, $"workspaces/{Encode(workspaceName)}/coveragestores.json");

    /// <summary>
    /// Gets one coverage store by name.
    /// </summary>
    public Task<CoverageStoreResponse> GetByNameAsync(string workspaceName, string storeName, CancellationToken cancellationToken = default)
        => SendAsync<CoverageStoreResponse>(HttpMethod.Get, $"workspaces/{Encode(workspaceName)}/coveragestores/{Encode(storeName)}.json", cancellationToken: cancellationToken);

    /// <summary>
    /// Gets one coverage store by name (synchronous).
    /// </summary>
    public CoverageStoreResponse GetByName(string workspaceName, string storeName)
        => Send<CoverageStoreResponse>(HttpMethod.Get, $"workspaces/{Encode(workspaceName)}/coveragestores/{Encode(storeName)}.json");

    /// <summary>
    /// Creates a coverage store.
    /// </summary>
    public Task CreateAsync(string workspaceName, CoverageStoreCreateRequest request, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Post, $"workspaces/{Encode(workspaceName)}/coveragestores", new { coverageStore = request }, cancellationToken);

    /// <summary>
    /// Creates a coverage store (synchronous).
    /// </summary>
    public void Create(string workspaceName, CoverageStoreCreateRequest request)
        => Send(HttpMethod.Post, $"workspaces/{Encode(workspaceName)}/coveragestores", new { coverageStore = request });

    /// <summary>
    /// Updates a coverage store.
    /// </summary>
    public Task UpdateAsync(string workspaceName, string storeName, CoverageStoreCreateRequest request, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Put, $"workspaces/{Encode(workspaceName)}/coveragestores/{Encode(storeName)}", new { coverageStore = request }, cancellationToken);

    /// <summary>
    /// Updates a coverage store (synchronous).
    /// </summary>
    public void Update(string workspaceName, string storeName, CoverageStoreCreateRequest request)
        => Send(HttpMethod.Put, $"workspaces/{Encode(workspaceName)}/coveragestores/{Encode(storeName)}", new { coverageStore = request });

    /// <summary>
    /// Deletes a coverage store.
    /// </summary>
    public Task DeleteAsync(string workspaceName, string storeName, bool recurse = false, bool purge = false, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Delete, $"workspaces/{Encode(workspaceName)}/coveragestores/{Encode(storeName)}?recurse={recurse.ToString().ToLowerInvariant()}&purge={(purge ? "all" : "none")}", cancellationToken: cancellationToken);

    /// <summary>
    /// Deletes a coverage store (synchronous).
    /// </summary>
    public void Delete(string workspaceName, string storeName, bool recurse = false, bool purge = false)
        => Send(HttpMethod.Delete, $"workspaces/{Encode(workspaceName)}/coveragestores/{Encode(storeName)}?recurse={recurse.ToString().ToLowerInvariant()}&purge={(purge ? "all" : "none")}");

    private static string Encode(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Value is required.", nameof(value));
        }

        return Uri.EscapeDataString(value);
    }
}
