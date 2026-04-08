using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using geoserver.net.Models.FeatureTypes;

namespace geoserver.net.Clients;

/// <summary>
/// Wrapper around GeoServer feature types REST endpoints.
/// </summary>
public sealed class FeatureTypesClient : GeoServerClientBase
{
    internal FeatureTypesClient(HttpClient httpClient) : base(httpClient)
    {
    }

    /// <summary>
    /// Gets feature types for a data store.
    /// </summary>
    public Task<FeatureTypeListResponse> GetAllAsync(string workspaceName, string dataStoreName, CancellationToken cancellationToken = default)
        => SendAsync<FeatureTypeListResponse>(HttpMethod.Get, $"workspaces/{Encode(workspaceName)}/datastores/{Encode(dataStoreName)}/featuretypes.json", cancellationToken: cancellationToken);

    /// <summary>
    /// Gets feature types for a data store (synchronous).
    /// </summary>
    public FeatureTypeListResponse GetAll(string workspaceName, string dataStoreName)
        => Send<FeatureTypeListResponse>(HttpMethod.Get, $"workspaces/{Encode(workspaceName)}/datastores/{Encode(dataStoreName)}/featuretypes.json");

    /// <summary>
    /// Gets one feature type.
    /// </summary>
    public Task<FeatureTypeResponse> GetByNameAsync(string workspaceName, string dataStoreName, string featureTypeName, CancellationToken cancellationToken = default)
        => SendAsync<FeatureTypeResponse>(HttpMethod.Get, $"workspaces/{Encode(workspaceName)}/datastores/{Encode(dataStoreName)}/featuretypes/{Encode(featureTypeName)}.json", cancellationToken: cancellationToken);

    /// <summary>
    /// Gets one feature type (synchronous).
    /// </summary>
    public FeatureTypeResponse GetByName(string workspaceName, string dataStoreName, string featureTypeName)
        => Send<FeatureTypeResponse>(HttpMethod.Get, $"workspaces/{Encode(workspaceName)}/datastores/{Encode(dataStoreName)}/featuretypes/{Encode(featureTypeName)}.json");

    /// <summary>
    /// Creates a feature type.
    /// </summary>
    public Task CreateAsync(string workspaceName, string dataStoreName, FeatureTypeCreateRequest request, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Post, $"workspaces/{Encode(workspaceName)}/datastores/{Encode(dataStoreName)}/featuretypes", new { featureType = request }, cancellationToken);

    /// <summary>
    /// Creates a feature type (synchronous).
    /// </summary>
    public void Create(string workspaceName, string dataStoreName, FeatureTypeCreateRequest request)
        => Send(HttpMethod.Post, $"workspaces/{Encode(workspaceName)}/datastores/{Encode(dataStoreName)}/featuretypes", new { featureType = request });

    /// <summary>
    /// Updates a feature type.
    /// </summary>
    public Task UpdateAsync(string workspaceName, string dataStoreName, string featureTypeName, FeatureTypeCreateRequest request, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Put, $"workspaces/{Encode(workspaceName)}/datastores/{Encode(dataStoreName)}/featuretypes/{Encode(featureTypeName)}", new { featureType = request }, cancellationToken);

    /// <summary>
    /// Updates a feature type (synchronous).
    /// </summary>
    public void Update(string workspaceName, string dataStoreName, string featureTypeName, FeatureTypeCreateRequest request)
        => Send(HttpMethod.Put, $"workspaces/{Encode(workspaceName)}/datastores/{Encode(dataStoreName)}/featuretypes/{Encode(featureTypeName)}", new { featureType = request });

    /// <summary>
    /// Deletes a feature type.
    /// </summary>
    public Task DeleteAsync(string workspaceName, string dataStoreName, string featureTypeName, bool recurse = false, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Delete, $"workspaces/{Encode(workspaceName)}/datastores/{Encode(dataStoreName)}/featuretypes/{Encode(featureTypeName)}?recurse={recurse.ToString().ToLowerInvariant()}", cancellationToken: cancellationToken);

    /// <summary>
    /// Deletes a feature type (synchronous).
    /// </summary>
    public void Delete(string workspaceName, string dataStoreName, string featureTypeName, bool recurse = false)
        => Send(HttpMethod.Delete, $"workspaces/{Encode(workspaceName)}/datastores/{Encode(dataStoreName)}/featuretypes/{Encode(featureTypeName)}?recurse={recurse.ToString().ToLowerInvariant()}");

    private static string Encode(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Value is required.", nameof(value));
        }

        return Uri.EscapeDataString(value);
    }
}
