using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using geoserver.net.Models.Wms;

namespace geoserver.net.Clients;

/// <summary>
/// Wrapper around GeoServer WMS layers endpoints.
/// </summary>
public sealed class WmsLayersClient : GeoServerClientBase
{
    internal WmsLayersClient(HttpClient httpClient) : base(httpClient)
    {
    }

    public Task<WmsLayerListResponse> GetAllAsync(string workspaceName, bool listAvailable = false, CancellationToken cancellationToken = default)
        => SendAsync<WmsLayerListResponse>(HttpMethod.Get, $"workspaces/{Encode(workspaceName)}/wmslayers.json{(listAvailable ? "?list=available" : string.Empty)}", cancellationToken: cancellationToken);

    public WmsLayerListResponse GetAll(string workspaceName, bool listAvailable = false)
        => Send<WmsLayerListResponse>(HttpMethod.Get, $"workspaces/{Encode(workspaceName)}/wmslayers.json{(listAvailable ? "?list=available" : string.Empty)}");

    public Task<WmsLayerListResponse> GetAllForStoreAsync(string workspaceName, string storeName, bool listAvailable = false, CancellationToken cancellationToken = default)
        => SendAsync<WmsLayerListResponse>(HttpMethod.Get, $"workspaces/{Encode(workspaceName)}/wmsstores/{Encode(storeName)}/wmslayers.json{(listAvailable ? "?list=available" : string.Empty)}", cancellationToken: cancellationToken);

    public WmsLayerListResponse GetAllForStore(string workspaceName, string storeName, bool listAvailable = false)
        => Send<WmsLayerListResponse>(HttpMethod.Get, $"workspaces/{Encode(workspaceName)}/wmsstores/{Encode(storeName)}/wmslayers.json{(listAvailable ? "?list=available" : string.Empty)}");

    public Task<WmsLayerResponse> GetByNameAsync(string workspaceName, string layerName, CancellationToken cancellationToken = default)
        => SendAsync<WmsLayerResponse>(HttpMethod.Get, $"workspaces/{Encode(workspaceName)}/wmslayers/{Encode(layerName)}.json", cancellationToken: cancellationToken);

    public WmsLayerResponse GetByName(string workspaceName, string layerName)
        => Send<WmsLayerResponse>(HttpMethod.Get, $"workspaces/{Encode(workspaceName)}/wmslayers/{Encode(layerName)}.json");

    public Task CreateAsync(string workspaceName, WmsLayerCreateRequest request, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Post, $"workspaces/{Encode(workspaceName)}/wmslayers", new { wmsLayer = request }, cancellationToken);

    public void Create(string workspaceName, WmsLayerCreateRequest request)
        => Send(HttpMethod.Post, $"workspaces/{Encode(workspaceName)}/wmslayers", new { wmsLayer = request });

    public Task CreateForStoreAsync(string workspaceName, string storeName, WmsLayerCreateRequest request, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Post, $"workspaces/{Encode(workspaceName)}/wmsstores/{Encode(storeName)}/wmslayers", new { wmsLayer = request }, cancellationToken);

    public void CreateForStore(string workspaceName, string storeName, WmsLayerCreateRequest request)
        => Send(HttpMethod.Post, $"workspaces/{Encode(workspaceName)}/wmsstores/{Encode(storeName)}/wmslayers", new { wmsLayer = request });

    public Task UpdateAsync(string workspaceName, string layerName, WmsLayerCreateRequest request, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Put, $"workspaces/{Encode(workspaceName)}/wmslayers/{Encode(layerName)}", new { wmsLayer = request }, cancellationToken);

    public void Update(string workspaceName, string layerName, WmsLayerCreateRequest request)
        => Send(HttpMethod.Put, $"workspaces/{Encode(workspaceName)}/wmslayers/{Encode(layerName)}", new { wmsLayer = request });

    public Task DeleteAsync(string workspaceName, string layerName, bool recurse = false, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Delete, $"workspaces/{Encode(workspaceName)}/wmslayers/{Encode(layerName)}?recurse={recurse.ToString().ToLowerInvariant()}", cancellationToken: cancellationToken);

    public void Delete(string workspaceName, string layerName, bool recurse = false)
        => Send(HttpMethod.Delete, $"workspaces/{Encode(workspaceName)}/wmslayers/{Encode(layerName)}?recurse={recurse.ToString().ToLowerInvariant()}");

    private static string Encode(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Value is required.", nameof(value));
        }

        return Uri.EscapeDataString(value);
    }
}
