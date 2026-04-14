using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using GeoServer.Models.StructuredCoverages;

namespace GeoServer.Clients;

/// <summary>
/// Wrapper around GeoServer structured coverages REST endpoints.
/// </summary>
public sealed class StructuredCoveragesClient : GeoServerClientBase
{
    internal StructuredCoveragesClient(HttpClient httpClient, GeoServerRequestContext? requestContext = null) : base(httpClient, requestContext)
    {
    }

    /// <summary>
    /// Gets structured coverage index schema.
    /// </summary>
    public Task<StructuredCoverageIndexResponse> GetIndexAsync(string workspaceName, string coverageStoreName, string coverageName, CancellationToken cancellationToken = default)
        => SendAsync<StructuredCoverageIndexResponse>(HttpMethod.Get, $"workspaces/{Encode(workspaceName)}/coveragestores/{Encode(coverageStoreName)}/coverages/{Encode(coverageName)}/index.json", cancellationToken: cancellationToken);

    /// <summary>
    /// Gets structured coverage index schema (synchronous).
    /// </summary>
    public StructuredCoverageIndexResponse GetIndex(string workspaceName, string coverageStoreName, string coverageName)
        => Send<StructuredCoverageIndexResponse>(HttpMethod.Get, $"workspaces/{Encode(workspaceName)}/coveragestores/{Encode(coverageStoreName)}/coverages/{Encode(coverageName)}/index.json");

    /// <summary>
    /// Gets granules for a structured coverage.
    /// </summary>
    public Task<StructuredCoverageGranulesResponse> GetGranulesAsync(string workspaceName, string coverageStoreName, string coverageName, string? cqlFilter = null, int? offset = null, int? limit = null, CancellationToken cancellationToken = default)
        => SendAsync<StructuredCoverageGranulesResponse>(HttpMethod.Get, BuildGranulesPath(workspaceName, coverageStoreName, coverageName, cqlFilter, offset, limit), cancellationToken: cancellationToken);

    /// <summary>
    /// Gets granules for a structured coverage (synchronous).
    /// </summary>
    public StructuredCoverageGranulesResponse GetGranules(string workspaceName, string coverageStoreName, string coverageName, string? cqlFilter = null, int? offset = null, int? limit = null)
        => Send<StructuredCoverageGranulesResponse>(HttpMethod.Get, BuildGranulesPath(workspaceName, coverageStoreName, coverageName, cqlFilter, offset, limit));

    /// <summary>
    /// Deletes granules that match optional filter.
    /// </summary>
    public Task DeleteGranulesAsync(string workspaceName, string coverageStoreName, string coverageName, string? cqlFilter = null, string purge = "none", bool updateBBox = false, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Delete, BuildDeleteGranulesPath(workspaceName, coverageStoreName, coverageName, cqlFilter, purge, updateBBox), cancellationToken: cancellationToken);

    /// <summary>
    /// Deletes granules that match optional filter (synchronous).
    /// </summary>
    public void DeleteGranules(string workspaceName, string coverageStoreName, string coverageName, string? cqlFilter = null, string purge = "none", bool updateBBox = false)
        => Send(HttpMethod.Delete, BuildDeleteGranulesPath(workspaceName, coverageStoreName, coverageName, cqlFilter, purge, updateBBox));

    /// <summary>
    /// Gets one granule by identifier.
    /// </summary>
    public Task<StructuredCoverageGranulesResponse> GetGranuleByIdAsync(string workspaceName, string coverageStoreName, string coverageName, string granuleId, CancellationToken cancellationToken = default)
        => SendAsync<StructuredCoverageGranulesResponse>(HttpMethod.Get, $"workspaces/{Encode(workspaceName)}/coveragestores/{Encode(coverageStoreName)}/coverages/{Encode(coverageName)}/index/granules/{Encode(granuleId)}.json", cancellationToken: cancellationToken);

    /// <summary>
    /// Gets one granule by identifier (synchronous).
    /// </summary>
    public StructuredCoverageGranulesResponse GetGranuleById(string workspaceName, string coverageStoreName, string coverageName, string granuleId)
        => Send<StructuredCoverageGranulesResponse>(HttpMethod.Get, $"workspaces/{Encode(workspaceName)}/coveragestores/{Encode(coverageStoreName)}/coverages/{Encode(coverageName)}/index/granules/{Encode(granuleId)}.json");

    private static string BuildGranulesPath(string workspaceName, string coverageStoreName, string coverageName, string? cqlFilter, int? offset, int? limit)
    {
        var path = $"workspaces/{Encode(workspaceName)}/coveragestores/{Encode(coverageStoreName)}/coverages/{Encode(coverageName)}/index/granules.json";
        return path + BuildQuery(cqlFilter, offset, limit, null, null);
    }

    private static string BuildDeleteGranulesPath(string workspaceName, string coverageStoreName, string coverageName, string? cqlFilter, string purge, bool updateBBox)
    {
        var path = $"workspaces/{Encode(workspaceName)}/coveragestores/{Encode(coverageStoreName)}/coverages/{Encode(coverageName)}/index/granules";
        return path + BuildQuery(cqlFilter, null, null, purge, updateBBox);
    }

    private static string BuildQuery(string? cqlFilter, int? offset, int? limit, string? purge, bool? updateBBox)
    {
        var hasAny = false;
        var query = string.Empty;

        void Add(string key, string value)
        {
            query += hasAny ? "&" : "?";
            query += $"{key}={Uri.EscapeDataString(value)}";
            hasAny = true;
        }

        if (!string.IsNullOrWhiteSpace(cqlFilter))
        {
            Add("filter", cqlFilter!);
        }

        if (offset.HasValue)
        {
            Add("offset", offset.Value.ToString(System.Globalization.CultureInfo.InvariantCulture));
        }

        if (limit.HasValue)
        {
            Add("limit", limit.Value.ToString(System.Globalization.CultureInfo.InvariantCulture));
        }

        if (!string.IsNullOrWhiteSpace(purge))
        {
            Add("purge", purge!);
        }

        if (updateBBox.HasValue)
        {
            Add("updateBBox", updateBBox.Value.ToString().ToLowerInvariant());
        }

        return query;
    }

    private static string Encode(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Value is required.", nameof(value));
        }

        return Uri.EscapeDataString(value);
    }
}
