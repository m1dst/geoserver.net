using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using geoserver.net.Models.Wmts;

namespace geoserver.net.Clients;

/// <summary>
/// Wrapper around GeoServer WMTS layers endpoints.
/// </summary>
public sealed class WmtsLayersClient : GeoServerClientBase
{
    internal WmtsLayersClient(HttpClient httpClient) : base(httpClient)
    {
    }

    /// <summary>
    /// Executes the GetAllAsync operation.
    /// </summary>
    public Task<WmtsLayerListResponse> GetAllAsync(string workspaceName, bool listAvailable = false, CancellationToken cancellationToken = default)
        => SendAsync<WmtsLayerListResponse>(HttpMethod.Get, $"workspaces/{Encode(workspaceName)}/wmtslayers.json{(listAvailable ? "?list=available" : string.Empty)}", cancellationToken: cancellationToken);

    /// <summary>
    /// Executes the GetAll operation.
    /// </summary>
    public WmtsLayerListResponse GetAll(string workspaceName, bool listAvailable = false)
        => Send<WmtsLayerListResponse>(HttpMethod.Get, $"workspaces/{Encode(workspaceName)}/wmtslayers.json{(listAvailable ? "?list=available" : string.Empty)}");

    /// <summary>
    /// Executes the GetAllForStoreAsync operation.
    /// </summary>
    public Task<WmtsLayerListResponse> GetAllForStoreAsync(string workspaceName, string storeName, bool listAvailable = false, CancellationToken cancellationToken = default)
        => SendAsync<WmtsLayerListResponse>(HttpMethod.Get, $"workspaces/{Encode(workspaceName)}/wmtsstores/{Encode(storeName)}/wmtslayers.json{(listAvailable ? "?list=available" : string.Empty)}", cancellationToken: cancellationToken);

    /// <summary>
    /// Executes the GetAllForStore operation.
    /// </summary>
    public WmtsLayerListResponse GetAllForStore(string workspaceName, string storeName, bool listAvailable = false)
        => Send<WmtsLayerListResponse>(HttpMethod.Get, $"workspaces/{Encode(workspaceName)}/wmtsstores/{Encode(storeName)}/wmtslayers.json{(listAvailable ? "?list=available" : string.Empty)}");

    /// <summary>
    /// Executes the GetByNameAsync operation.
    /// </summary>
    public Task<WmtsLayerResponse> GetByNameAsync(string workspaceName, string layerName, CancellationToken cancellationToken = default)
        => SendAsync<WmtsLayerResponse>(HttpMethod.Get, $"workspaces/{Encode(workspaceName)}/wmtslayers/{Encode(layerName)}.json", cancellationToken: cancellationToken);

    /// <summary>
    /// Executes the GetByName operation.
    /// </summary>
    public WmtsLayerResponse GetByName(string workspaceName, string layerName)
        => Send<WmtsLayerResponse>(HttpMethod.Get, $"workspaces/{Encode(workspaceName)}/wmtslayers/{Encode(layerName)}.json");

    /// <summary>
    /// Executes the CreateAsync operation.
    /// </summary>
    public Task CreateAsync(string workspaceName, WmtsLayerCreateRequest request, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Post, $"workspaces/{Encode(workspaceName)}/wmtslayers", new { wmtsLayer = request }, cancellationToken);

    /// <summary>
    /// Executes the Create operation.
    /// </summary>
    public void Create(string workspaceName, WmtsLayerCreateRequest request)
        => Send(HttpMethod.Post, $"workspaces/{Encode(workspaceName)}/wmtslayers", new { wmtsLayer = request });

    /// <summary>
    /// Executes the CreateForStoreAsync operation.
    /// </summary>
    public Task CreateForStoreAsync(string workspaceName, string storeName, WmtsLayerCreateRequest request, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Post, $"workspaces/{Encode(workspaceName)}/wmtsstores/{Encode(storeName)}/wmtslayers", new { wmtsLayer = request }, cancellationToken);

    /// <summary>
    /// Executes the CreateForStore operation.
    /// </summary>
    public void CreateForStore(string workspaceName, string storeName, WmtsLayerCreateRequest request)
        => Send(HttpMethod.Post, $"workspaces/{Encode(workspaceName)}/wmtsstores/{Encode(storeName)}/wmtslayers", new { wmtsLayer = request });

    /// <summary>
    /// Executes the UpdateAsync operation.
    /// </summary>
    public Task UpdateAsync(string workspaceName, string layerName, WmtsLayerCreateRequest request, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Put, $"workspaces/{Encode(workspaceName)}/wmtslayers/{Encode(layerName)}", new { wmtsLayer = request }, cancellationToken);

    /// <summary>
    /// Executes the Update operation.
    /// </summary>
    public void Update(string workspaceName, string layerName, WmtsLayerCreateRequest request)
        => Send(HttpMethod.Put, $"workspaces/{Encode(workspaceName)}/wmtslayers/{Encode(layerName)}", new { wmtsLayer = request });

    /// <summary>
    /// Executes the DeleteAsync operation.
    /// </summary>
    public Task DeleteAsync(string workspaceName, string layerName, bool recurse = false, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Delete, $"workspaces/{Encode(workspaceName)}/wmtslayers/{Encode(layerName)}?recurse={recurse.ToString().ToLowerInvariant()}", cancellationToken: cancellationToken);

    /// <summary>
    /// Executes the Delete operation.
    /// </summary>
    public void Delete(string workspaceName, string layerName, bool recurse = false)
        => Send(HttpMethod.Delete, $"workspaces/{Encode(workspaceName)}/wmtslayers/{Encode(layerName)}?recurse={recurse.ToString().ToLowerInvariant()}");

    private static string Encode(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Value is required.", nameof(value));
        }

        return Uri.EscapeDataString(value);
    }
}
