using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using geoserver.net.Models.Wms;

namespace geoserver.net.Clients;

/// <summary>
/// Wrapper around GeoServer WMS stores endpoints.
/// </summary>
public sealed class WmsStoresClient : GeoServerClientBase
{
    internal WmsStoresClient(HttpClient httpClient, GeoServerRequestContext? requestContext = null) : base(httpClient, requestContext)
    {
    }

    /// <summary>
    /// Executes the GetAllAsync operation.
    /// </summary>
    public Task<WmsStoreListResponse> GetAllAsync(string workspaceName, CancellationToken cancellationToken = default)
        => SendAsync<WmsStoreListResponse>(HttpMethod.Get, $"workspaces/{Encode(workspaceName)}/wmsstores.json", cancellationToken: cancellationToken);

    /// <summary>
    /// Executes the GetAll operation.
    /// </summary>
    public WmsStoreListResponse GetAll(string workspaceName)
        => Send<WmsStoreListResponse>(HttpMethod.Get, $"workspaces/{Encode(workspaceName)}/wmsstores.json");

    /// <summary>
    /// Executes the GetByNameAsync operation.
    /// </summary>
    public Task<WmsStoreResponse> GetByNameAsync(string workspaceName, string storeName, CancellationToken cancellationToken = default)
        => SendAsync<WmsStoreResponse>(HttpMethod.Get, $"workspaces/{Encode(workspaceName)}/wmsstores/{Encode(storeName)}.json", cancellationToken: cancellationToken);

    /// <summary>
    /// Executes the GetByName operation.
    /// </summary>
    public WmsStoreResponse GetByName(string workspaceName, string storeName)
        => Send<WmsStoreResponse>(HttpMethod.Get, $"workspaces/{Encode(workspaceName)}/wmsstores/{Encode(storeName)}.json");

    /// <summary>
    /// Executes the CreateAsync operation.
    /// </summary>
    public Task CreateAsync(string workspaceName, WmsStoreCreateRequest request, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Post, $"workspaces/{Encode(workspaceName)}/wmsstores", new { wmsStore = request }, cancellationToken);

    /// <summary>
    /// Executes the Create operation.
    /// </summary>
    public void Create(string workspaceName, WmsStoreCreateRequest request)
        => Send(HttpMethod.Post, $"workspaces/{Encode(workspaceName)}/wmsstores", new { wmsStore = request });

    /// <summary>
    /// Executes the UpdateAsync operation.
    /// </summary>
    public Task UpdateAsync(string workspaceName, string storeName, WmsStoreCreateRequest request, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Put, $"workspaces/{Encode(workspaceName)}/wmsstores/{Encode(storeName)}", new { wmsStore = request }, cancellationToken);

    /// <summary>
    /// Executes the Update operation.
    /// </summary>
    public void Update(string workspaceName, string storeName, WmsStoreCreateRequest request)
        => Send(HttpMethod.Put, $"workspaces/{Encode(workspaceName)}/wmsstores/{Encode(storeName)}", new { wmsStore = request });

    /// <summary>
    /// Executes the DeleteAsync operation.
    /// </summary>
    public Task DeleteAsync(string workspaceName, string storeName, bool recurse = false, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Delete, $"workspaces/{Encode(workspaceName)}/wmsstores/{Encode(storeName)}?recurse={recurse.ToString().ToLowerInvariant()}", cancellationToken: cancellationToken);

    /// <summary>
    /// Executes the Delete operation.
    /// </summary>
    public void Delete(string workspaceName, string storeName, bool recurse = false)
        => Send(HttpMethod.Delete, $"workspaces/{Encode(workspaceName)}/wmsstores/{Encode(storeName)}?recurse={recurse.ToString().ToLowerInvariant()}");

    private static string Encode(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Value is required.", nameof(value));
        }

        return Uri.EscapeDataString(value);
    }
}
