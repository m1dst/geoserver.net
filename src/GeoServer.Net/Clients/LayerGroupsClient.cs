using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using geoserver.net.Models.LayerGroups;

namespace geoserver.net.Clients;

/// <summary>
/// Wrapper around GeoServer layer groups REST endpoints.
/// </summary>
public sealed class LayerGroupsClient : GeoServerClientBase
{
    internal LayerGroupsClient(HttpClient httpClient) : base(httpClient)
    {
    }

    /// <summary>
    /// Gets all layer groups.
    /// </summary>
    public Task<LayerGroupListResponse> GetAllAsync(CancellationToken cancellationToken = default)
        => SendAsync<LayerGroupListResponse>(HttpMethod.Get, "layergroups.json", cancellationToken: cancellationToken);

    /// <summary>
    /// Gets all layer groups (synchronous).
    /// </summary>
    public LayerGroupListResponse GetAll() => Send<LayerGroupListResponse>(HttpMethod.Get, "layergroups.json");

    /// <summary>
    /// Gets one layer group by name.
    /// </summary>
    public Task<LayerGroupResponse> GetByNameAsync(string layerGroupName, CancellationToken cancellationToken = default)
        => SendAsync<LayerGroupResponse>(HttpMethod.Get, $"layergroups/{Encode(layerGroupName)}.json", cancellationToken: cancellationToken);

    /// <summary>
    /// Gets one layer group by name (synchronous).
    /// </summary>
    public LayerGroupResponse GetByName(string layerGroupName)
        => Send<LayerGroupResponse>(HttpMethod.Get, $"layergroups/{Encode(layerGroupName)}.json");

    /// <summary>
    /// Creates a layer group.
    /// </summary>
    public Task CreateAsync(LayerGroupCreateRequest request, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Post, "layergroups", new { layerGroup = request }, cancellationToken);

    /// <summary>
    /// Creates a layer group (synchronous).
    /// </summary>
    public void Create(LayerGroupCreateRequest request)
        => Send(HttpMethod.Post, "layergroups", new { layerGroup = request });

    /// <summary>
    /// Updates a layer group.
    /// </summary>
    public Task UpdateAsync(string layerGroupName, LayerGroupCreateRequest request, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Put, $"layergroups/{Encode(layerGroupName)}", new { layerGroup = request }, cancellationToken);

    /// <summary>
    /// Updates a layer group (synchronous).
    /// </summary>
    public void Update(string layerGroupName, LayerGroupCreateRequest request)
        => Send(HttpMethod.Put, $"layergroups/{Encode(layerGroupName)}", new { layerGroup = request });

    /// <summary>
    /// Deletes a layer group.
    /// </summary>
    public Task DeleteAsync(string layerGroupName, bool recurse = false, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Delete, $"layergroups/{Encode(layerGroupName)}?recurse={recurse.ToString().ToLowerInvariant()}", cancellationToken: cancellationToken);

    /// <summary>
    /// Deletes a layer group (synchronous).
    /// </summary>
    public void Delete(string layerGroupName, bool recurse = false)
        => Send(HttpMethod.Delete, $"layergroups/{Encode(layerGroupName)}?recurse={recurse.ToString().ToLowerInvariant()}");

    private static string Encode(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Value is required.", nameof(value));
        }

        return Uri.EscapeDataString(value);
    }
}
