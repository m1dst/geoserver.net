using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using geoserver.net.Models.Layers;

namespace geoserver.net.Clients;

/// <summary>
/// Wrapper around GeoServer layers REST endpoints.
/// </summary>
public sealed class LayersClient : GeoServerClientBase
{
    internal LayersClient(HttpClient httpClient) : base(httpClient)
    {
    }

    /// <summary>
    /// Gets all layers.
    /// </summary>
    public Task<LayerListResponse> GetAllAsync(CancellationToken cancellationToken = default)
        => SendAsync<LayerListResponse>(HttpMethod.Get, "layers.json", cancellationToken: cancellationToken);

    /// <summary>
    /// Gets all layers (synchronous).
    /// </summary>
    public LayerListResponse GetAll() => Send<LayerListResponse>(HttpMethod.Get, "layers.json");

    /// <summary>
    /// Gets one layer by name.
    /// </summary>
    public Task<LayerResponse> GetByNameAsync(string layerName, CancellationToken cancellationToken = default)
        => SendAsync<LayerResponse>(HttpMethod.Get, $"layers/{Encode(layerName)}.json", cancellationToken: cancellationToken);

    /// <summary>
    /// Gets one layer by name (synchronous).
    /// </summary>
    public LayerResponse GetByName(string layerName)
        => Send<LayerResponse>(HttpMethod.Get, $"layers/{Encode(layerName)}.json");

    /// <summary>
    /// Updates a layer.
    /// </summary>
    public Task UpdateAsync(string layerName, LayerUpdateRequest request, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Put, $"layers/{Encode(layerName)}", new { layer = request }, cancellationToken);

    /// <summary>
    /// Updates a layer (synchronous).
    /// </summary>
    public void Update(string layerName, LayerUpdateRequest request)
        => Send(HttpMethod.Put, $"layers/{Encode(layerName)}", new { layer = request });

    /// <summary>
    /// Deletes a layer.
    /// </summary>
    public Task DeleteAsync(string layerName, bool recurse = false, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Delete, $"layers/{Encode(layerName)}?recurse={recurse.ToString().ToLowerInvariant()}", cancellationToken: cancellationToken);

    /// <summary>
    /// Deletes a layer (synchronous).
    /// </summary>
    public void Delete(string layerName, bool recurse = false)
        => Send(HttpMethod.Delete, $"layers/{Encode(layerName)}?recurse={recurse.ToString().ToLowerInvariant()}");

    private static string Encode(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Value is required.", nameof(value));
        }

        return Uri.EscapeDataString(value);
    }
}
