using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using GeoServer.Models.Wms;

namespace GeoServer.Clients;

/// <summary>
/// Wrapper around GeoServer WMS layers endpoints.
/// </summary>
public sealed class WmsLayersClient : GeoServerClientBase
{
    internal WmsLayersClient(HttpClient httpClient, GeoServerRequestContext? requestContext = null) : base(httpClient, requestContext)
    {
    }

    /// <summary>
    /// Executes the GetAllAsync operation.
    /// </summary>
    public Task<WmsLayerListResponse> GetAllAsync(string workspaceName, bool listAvailable = false, CancellationToken cancellationToken = default)
        => SendAsync<WmsLayerListResponse>(HttpMethod.Get, $"workspaces/{Encode(workspaceName)}/wmslayers.json{(listAvailable ? "?list=available" : string.Empty)}", cancellationToken: cancellationToken);

    /// <summary>
    /// Executes the GetAll operation.
    /// </summary>
    public WmsLayerListResponse GetAll(string workspaceName, bool listAvailable = false)
        => Send<WmsLayerListResponse>(HttpMethod.Get, $"workspaces/{Encode(workspaceName)}/wmslayers.json{(listAvailable ? "?list=available" : string.Empty)}");

    /// <summary>
    /// Executes the GetAllForStoreAsync operation.
    /// </summary>
    public Task<WmsLayerListResponse> GetAllForStoreAsync(string workspaceName, string storeName, bool listAvailable = false, CancellationToken cancellationToken = default)
        => SendAsync<WmsLayerListResponse>(HttpMethod.Get, $"workspaces/{Encode(workspaceName)}/wmsstores/{Encode(storeName)}/wmslayers.json{(listAvailable ? "?list=available" : string.Empty)}", cancellationToken: cancellationToken);

    /// <summary>
    /// Executes the GetAllForStore operation.
    /// </summary>
    public WmsLayerListResponse GetAllForStore(string workspaceName, string storeName, bool listAvailable = false)
        => Send<WmsLayerListResponse>(HttpMethod.Get, $"workspaces/{Encode(workspaceName)}/wmsstores/{Encode(storeName)}/wmslayers.json{(listAvailable ? "?list=available" : string.Empty)}");

    /// <summary>
    /// Executes the GetByNameAsync operation.
    /// </summary>
    public Task<WmsLayerResponse> GetByNameAsync(string workspaceName, string layerName, CancellationToken cancellationToken = default)
        => SendAsync<WmsLayerResponse>(HttpMethod.Get, $"workspaces/{Encode(workspaceName)}/wmslayers/{Encode(layerName)}.json", cancellationToken: cancellationToken);

    /// <summary>
    /// Executes the GetByName operation.
    /// </summary>
    public WmsLayerResponse GetByName(string workspaceName, string layerName)
        => Send<WmsLayerResponse>(HttpMethod.Get, $"workspaces/{Encode(workspaceName)}/wmslayers/{Encode(layerName)}.json");

    /// <summary>
    /// Executes the CreateAsync operation.
    /// </summary>
    public Task CreateAsync(string workspaceName, WmsLayerCreateRequest request, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Post, $"workspaces/{Encode(workspaceName)}/wmslayers", new { wmsLayer = request }, cancellationToken);

    /// <summary>
    /// Executes the Create operation.
    /// </summary>
    public void Create(string workspaceName, WmsLayerCreateRequest request)
        => Send(HttpMethod.Post, $"workspaces/{Encode(workspaceName)}/wmslayers", new { wmsLayer = request });

    /// <summary>
    /// Executes the CreateForStoreAsync operation.
    /// </summary>
    public Task CreateForStoreAsync(string workspaceName, string storeName, WmsLayerCreateRequest request, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Post, $"workspaces/{Encode(workspaceName)}/wmsstores/{Encode(storeName)}/wmslayers", new { wmsLayer = request }, cancellationToken);

    /// <summary>
    /// Executes the CreateForStore operation.
    /// </summary>
    public void CreateForStore(string workspaceName, string storeName, WmsLayerCreateRequest request)
        => Send(HttpMethod.Post, $"workspaces/{Encode(workspaceName)}/wmsstores/{Encode(storeName)}/wmslayers", new { wmsLayer = request });

    /// <summary>
    /// Executes the UpdateAsync operation.
    /// </summary>
    public Task UpdateAsync(string workspaceName, string layerName, WmsLayerCreateRequest request, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Put, $"workspaces/{Encode(workspaceName)}/wmslayers/{Encode(layerName)}", new { wmsLayer = request }, cancellationToken);

    /// <summary>
    /// Executes the Update operation.
    /// </summary>
    public void Update(string workspaceName, string layerName, WmsLayerCreateRequest request)
        => Send(HttpMethod.Put, $"workspaces/{Encode(workspaceName)}/wmslayers/{Encode(layerName)}", new { wmsLayer = request });

    /// <summary>
    /// Executes the DeleteAsync operation.
    /// </summary>
    public Task DeleteAsync(string workspaceName, string layerName, bool recurse = false, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Delete, $"workspaces/{Encode(workspaceName)}/wmslayers/{Encode(layerName)}?recurse={recurse.ToString().ToLowerInvariant()}", cancellationToken: cancellationToken);

    /// <summary>
    /// Executes the Delete operation.
    /// </summary>
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
