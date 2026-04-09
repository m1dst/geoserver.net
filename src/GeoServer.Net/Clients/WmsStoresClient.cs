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
    internal WmsStoresClient(HttpClient httpClient) : base(httpClient)
    {
    }

    public Task<WmsStoreListResponse> GetAllAsync(string workspaceName, CancellationToken cancellationToken = default)
        => SendAsync<WmsStoreListResponse>(HttpMethod.Get, $"workspaces/{Encode(workspaceName)}/wmsstores.json", cancellationToken: cancellationToken);

    public WmsStoreListResponse GetAll(string workspaceName)
        => Send<WmsStoreListResponse>(HttpMethod.Get, $"workspaces/{Encode(workspaceName)}/wmsstores.json");

    public Task<WmsStoreResponse> GetByNameAsync(string workspaceName, string storeName, CancellationToken cancellationToken = default)
        => SendAsync<WmsStoreResponse>(HttpMethod.Get, $"workspaces/{Encode(workspaceName)}/wmsstores/{Encode(storeName)}.json", cancellationToken: cancellationToken);

    public WmsStoreResponse GetByName(string workspaceName, string storeName)
        => Send<WmsStoreResponse>(HttpMethod.Get, $"workspaces/{Encode(workspaceName)}/wmsstores/{Encode(storeName)}.json");

    public Task CreateAsync(string workspaceName, WmsStoreCreateRequest request, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Post, $"workspaces/{Encode(workspaceName)}/wmsstores", new { wmsStore = request }, cancellationToken);

    public void Create(string workspaceName, WmsStoreCreateRequest request)
        => Send(HttpMethod.Post, $"workspaces/{Encode(workspaceName)}/wmsstores", new { wmsStore = request });

    public Task UpdateAsync(string workspaceName, string storeName, WmsStoreCreateRequest request, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Put, $"workspaces/{Encode(workspaceName)}/wmsstores/{Encode(storeName)}", new { wmsStore = request }, cancellationToken);

    public void Update(string workspaceName, string storeName, WmsStoreCreateRequest request)
        => Send(HttpMethod.Put, $"workspaces/{Encode(workspaceName)}/wmsstores/{Encode(storeName)}", new { wmsStore = request });

    public Task DeleteAsync(string workspaceName, string storeName, bool recurse = false, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Delete, $"workspaces/{Encode(workspaceName)}/wmsstores/{Encode(storeName)}?recurse={recurse.ToString().ToLowerInvariant()}", cancellationToken: cancellationToken);

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
