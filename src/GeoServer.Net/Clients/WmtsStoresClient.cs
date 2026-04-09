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
    internal WmtsStoresClient(HttpClient httpClient) : base(httpClient)
    {
    }

    public Task<WmtsStoreListResponse> GetAllAsync(string workspaceName, CancellationToken cancellationToken = default)
        => SendAsync<WmtsStoreListResponse>(HttpMethod.Get, $"workspaces/{Encode(workspaceName)}/wmtsstores.json", cancellationToken: cancellationToken);

    public WmtsStoreListResponse GetAll(string workspaceName)
        => Send<WmtsStoreListResponse>(HttpMethod.Get, $"workspaces/{Encode(workspaceName)}/wmtsstores.json");

    public Task<WmtsStoreResponse> GetByNameAsync(string workspaceName, string storeName, CancellationToken cancellationToken = default)
        => SendAsync<WmtsStoreResponse>(HttpMethod.Get, $"workspaces/{Encode(workspaceName)}/wmtsstores/{Encode(storeName)}.json", cancellationToken: cancellationToken);

    public WmtsStoreResponse GetByName(string workspaceName, string storeName)
        => Send<WmtsStoreResponse>(HttpMethod.Get, $"workspaces/{Encode(workspaceName)}/wmtsstores/{Encode(storeName)}.json");

    public Task CreateAsync(string workspaceName, WmtsStoreCreateRequest request, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Post, $"workspaces/{Encode(workspaceName)}/wmtsstores", new { wmtsStore = request }, cancellationToken);

    public void Create(string workspaceName, WmtsStoreCreateRequest request)
        => Send(HttpMethod.Post, $"workspaces/{Encode(workspaceName)}/wmtsstores", new { wmtsStore = request });

    public Task UpdateAsync(string workspaceName, string storeName, WmtsStoreCreateRequest request, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Put, $"workspaces/{Encode(workspaceName)}/wmtsstores/{Encode(storeName)}", new { wmtsStore = request }, cancellationToken);

    public void Update(string workspaceName, string storeName, WmtsStoreCreateRequest request)
        => Send(HttpMethod.Put, $"workspaces/{Encode(workspaceName)}/wmtsstores/{Encode(storeName)}", new { wmtsStore = request });

    public Task DeleteAsync(string workspaceName, string storeName, bool recurse = false, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Delete, $"workspaces/{Encode(workspaceName)}/wmtsstores/{Encode(storeName)}?recurse={recurse.ToString().ToLowerInvariant()}", cancellationToken: cancellationToken);

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
