using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using geoserver.net.Models.Wmts;

namespace geoserver.net.Clients;

/// <summary>
/// Wrapper around GeoServer WMTS stores endpoints.
/// </summary>
public sealed class WmtsStoresClient : GeoServerClientBase
{
    internal WmtsStoresClient(HttpClient httpClient, GeoServerRequestContext? requestContext = null) : base(httpClient, requestContext)
    {
    }

    /// <summary>
    /// Executes the GetAllAsync operation.
    /// </summary>
    public Task<WmtsStoreListResponse> GetAllAsync(string workspaceName, CancellationToken cancellationToken = default)
        => SendAsync<WmtsStoreListResponse>(HttpMethod.Get, $"workspaces/{Encode(workspaceName)}/wmtsstores.json", cancellationToken: cancellationToken);

    /// <summary>
    /// Executes the GetAll operation.
    /// </summary>
    public WmtsStoreListResponse GetAll(string workspaceName)
        => Send<WmtsStoreListResponse>(HttpMethod.Get, $"workspaces/{Encode(workspaceName)}/wmtsstores.json");

    /// <summary>
    /// Executes the GetByNameAsync operation.
    /// </summary>
    public Task<WmtsStoreResponse> GetByNameAsync(string workspaceName, string storeName, CancellationToken cancellationToken = default)
        => SendAsync<WmtsStoreResponse>(HttpMethod.Get, $"workspaces/{Encode(workspaceName)}/wmtsstores/{Encode(storeName)}.json", cancellationToken: cancellationToken);

    /// <summary>
    /// Executes the GetByName operation.
    /// </summary>
    public WmtsStoreResponse GetByName(string workspaceName, string storeName)
        => Send<WmtsStoreResponse>(HttpMethod.Get, $"workspaces/{Encode(workspaceName)}/wmtsstores/{Encode(storeName)}.json");

    /// <summary>
    /// Executes the CreateAsync operation.
    /// </summary>
    public Task CreateAsync(string workspaceName, WmtsStoreCreateRequest request, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Post, $"workspaces/{Encode(workspaceName)}/wmtsstores", new { wmtsStore = request }, cancellationToken);

    /// <summary>
    /// Executes the Create operation.
    /// </summary>
    public void Create(string workspaceName, WmtsStoreCreateRequest request)
        => Send(HttpMethod.Post, $"workspaces/{Encode(workspaceName)}/wmtsstores", new { wmtsStore = request });

    /// <summary>
    /// Executes the UpdateAsync operation.
    /// </summary>
    public Task UpdateAsync(string workspaceName, string storeName, WmtsStoreCreateRequest request, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Put, $"workspaces/{Encode(workspaceName)}/wmtsstores/{Encode(storeName)}", new { wmtsStore = request }, cancellationToken);

    /// <summary>
    /// Executes the Update operation.
    /// </summary>
    public void Update(string workspaceName, string storeName, WmtsStoreCreateRequest request)
        => Send(HttpMethod.Put, $"workspaces/{Encode(workspaceName)}/wmtsstores/{Encode(storeName)}", new { wmtsStore = request });

    /// <summary>
    /// Executes the DeleteAsync operation.
    /// </summary>
    public Task DeleteAsync(string workspaceName, string storeName, bool recurse = false, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Delete, $"workspaces/{Encode(workspaceName)}/wmtsstores/{Encode(storeName)}?recurse={recurse.ToString().ToLowerInvariant()}", cancellationToken: cancellationToken);

    /// <summary>
    /// Executes the Delete operation.
    /// </summary>
    public void Delete(string workspaceName, string storeName, bool recurse = false)
        => Send(HttpMethod.Delete, $"workspaces/{Encode(workspaceName)}/wmtsstores/{Encode(storeName)}?recurse={recurse.ToString().ToLowerInvariant()}");

    private static string Encode(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Value is required.", nameof(value));
        }

        return Uri.EscapeDataString(value);
    }
}
