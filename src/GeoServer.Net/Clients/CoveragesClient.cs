using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using GeoServer.Models.Coverages;

namespace GeoServer.Clients;

/// <summary>
/// Wrapper around GeoServer coverages REST endpoints.
/// </summary>
public sealed class CoveragesClient : GeoServerClientBase
{
    internal CoveragesClient(HttpClient httpClient, GeoServerRequestContext? requestContext = null) : base(httpClient, requestContext)
    {
    }

    /// <summary>
    /// Gets coverages in a coverage store.
    /// </summary>
    public Task<CoverageListResponse> GetAllAsync(string workspaceName, string coverageStoreName, CancellationToken cancellationToken = default)
        => SendAsync<CoverageListResponse>(HttpMethod.Get, $"workspaces/{Encode(workspaceName)}/coveragestores/{Encode(coverageStoreName)}/coverages.json", cancellationToken: cancellationToken);

    /// <summary>
    /// Gets coverages in a coverage store (synchronous).
    /// </summary>
    public CoverageListResponse GetAll(string workspaceName, string coverageStoreName)
        => Send<CoverageListResponse>(HttpMethod.Get, $"workspaces/{Encode(workspaceName)}/coveragestores/{Encode(coverageStoreName)}/coverages.json");

    /// <summary>
    /// Gets one coverage by name.
    /// </summary>
    public Task<CoverageResponse> GetByNameAsync(string workspaceName, string coverageStoreName, string coverageName, CancellationToken cancellationToken = default)
        => SendAsync<CoverageResponse>(HttpMethod.Get, $"workspaces/{Encode(workspaceName)}/coveragestores/{Encode(coverageStoreName)}/coverages/{Encode(coverageName)}.json", cancellationToken: cancellationToken);

    /// <summary>
    /// Gets one coverage by name (synchronous).
    /// </summary>
    public CoverageResponse GetByName(string workspaceName, string coverageStoreName, string coverageName)
        => Send<CoverageResponse>(HttpMethod.Get, $"workspaces/{Encode(workspaceName)}/coveragestores/{Encode(coverageStoreName)}/coverages/{Encode(coverageName)}.json");

    /// <summary>
    /// Creates a coverage.
    /// </summary>
    public Task CreateAsync(string workspaceName, string coverageStoreName, CoverageCreateRequest request, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Post, $"workspaces/{Encode(workspaceName)}/coveragestores/{Encode(coverageStoreName)}/coverages", new { coverage = request }, cancellationToken);

    /// <summary>
    /// Creates a coverage (synchronous).
    /// </summary>
    public void Create(string workspaceName, string coverageStoreName, CoverageCreateRequest request)
        => Send(HttpMethod.Post, $"workspaces/{Encode(workspaceName)}/coveragestores/{Encode(coverageStoreName)}/coverages", new { coverage = request });

    /// <summary>
    /// Updates a coverage.
    /// </summary>
    public Task UpdateAsync(string workspaceName, string coverageStoreName, string coverageName, CoverageCreateRequest request, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Put, $"workspaces/{Encode(workspaceName)}/coveragestores/{Encode(coverageStoreName)}/coverages/{Encode(coverageName)}", new { coverage = request }, cancellationToken);

    /// <summary>
    /// Updates a coverage (synchronous).
    /// </summary>
    public void Update(string workspaceName, string coverageStoreName, string coverageName, CoverageCreateRequest request)
        => Send(HttpMethod.Put, $"workspaces/{Encode(workspaceName)}/coveragestores/{Encode(coverageStoreName)}/coverages/{Encode(coverageName)}", new { coverage = request });

    /// <summary>
    /// Deletes a coverage.
    /// </summary>
    public Task DeleteAsync(string workspaceName, string coverageStoreName, string coverageName, bool recurse = false, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Delete, $"workspaces/{Encode(workspaceName)}/coveragestores/{Encode(coverageStoreName)}/coverages/{Encode(coverageName)}?recurse={recurse.ToString().ToLowerInvariant()}", cancellationToken: cancellationToken);

    /// <summary>
    /// Deletes a coverage (synchronous).
    /// </summary>
    public void Delete(string workspaceName, string coverageStoreName, string coverageName, bool recurse = false)
        => Send(HttpMethod.Delete, $"workspaces/{Encode(workspaceName)}/coveragestores/{Encode(coverageStoreName)}/coverages/{Encode(coverageName)}?recurse={recurse.ToString().ToLowerInvariant()}");

    private static string Encode(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Value is required.", nameof(value));
        }

        return Uri.EscapeDataString(value);
    }
}
