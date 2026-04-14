using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using GeoServer.Models.Stores;

namespace GeoServer.Clients;

/// <summary>
/// Wrapper around GeoServer data stores REST endpoints.
/// </summary>
public sealed class DataStoresClient : GeoServerClientBase
{
    internal DataStoresClient(HttpClient httpClient, GeoServerRequestContext? requestContext = null) : base(httpClient, requestContext)
    {
    }

    /// <summary>
    /// Gets data stores for a workspace.
    /// </summary>
    public Task<DataStoreListResponse> GetAllAsync(string workspaceName, CancellationToken cancellationToken = default)
        => SendAsync<DataStoreListResponse>(HttpMethod.Get, $"workspaces/{Encode(workspaceName)}/datastores.json", cancellationToken: cancellationToken);

    /// <summary>
    /// Gets data stores for a workspace (synchronous).
    /// </summary>
    public DataStoreListResponse GetAll(string workspaceName)
        => Send<DataStoreListResponse>(HttpMethod.Get, $"workspaces/{Encode(workspaceName)}/datastores.json");

    /// <summary>
    /// Gets one data store by name.
    /// </summary>
    public Task<DataStoreResponse> GetByNameAsync(string workspaceName, string storeName, CancellationToken cancellationToken = default)
        => SendAsync<DataStoreResponse>(HttpMethod.Get, $"workspaces/{Encode(workspaceName)}/datastores/{Encode(storeName)}.json", cancellationToken: cancellationToken);

    /// <summary>
    /// Gets one data store by name (synchronous).
    /// </summary>
    public DataStoreResponse GetByName(string workspaceName, string storeName)
        => Send<DataStoreResponse>(HttpMethod.Get, $"workspaces/{Encode(workspaceName)}/datastores/{Encode(storeName)}.json");

    /// <summary>
    /// Creates a data store.
    /// </summary>
    public Task CreateAsync(string workspaceName, DataStoreCreateRequest request, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Post, $"workspaces/{Encode(workspaceName)}/datastores", new { dataStore = request }, cancellationToken);

    /// <summary>
    /// Creates a data store (synchronous).
    /// </summary>
    public void Create(string workspaceName, DataStoreCreateRequest request)
        => Send(HttpMethod.Post, $"workspaces/{Encode(workspaceName)}/datastores", new { dataStore = request });

    /// <summary>
    /// Updates a data store.
    /// </summary>
    public Task UpdateAsync(string workspaceName, string storeName, DataStoreCreateRequest request, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Put, $"workspaces/{Encode(workspaceName)}/datastores/{Encode(storeName)}", new { dataStore = request }, cancellationToken);

    /// <summary>
    /// Updates a data store (synchronous).
    /// </summary>
    public void Update(string workspaceName, string storeName, DataStoreCreateRequest request)
        => Send(HttpMethod.Put, $"workspaces/{Encode(workspaceName)}/datastores/{Encode(storeName)}", new { dataStore = request });

    /// <summary>
    /// Deletes a data store.
    /// </summary>
    public Task DeleteAsync(string workspaceName, string storeName, bool recurse = false, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Delete, $"workspaces/{Encode(workspaceName)}/datastores/{Encode(storeName)}?recurse={recurse.ToString().ToLowerInvariant()}", cancellationToken: cancellationToken);

    /// <summary>
    /// Deletes a data store (synchronous).
    /// </summary>
    public void Delete(string workspaceName, string storeName, bool recurse = false)
        => Send(HttpMethod.Delete, $"workspaces/{Encode(workspaceName)}/datastores/{Encode(storeName)}?recurse={recurse.ToString().ToLowerInvariant()}");

    private static string Encode(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Value is required.", nameof(value));
        }

        return Uri.EscapeDataString(value);
    }
}
