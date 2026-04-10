using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using geoserver.net.Models.Crs;

namespace geoserver.net.Clients;

/// <summary>
/// Wrapper around GeoServer CRS endpoints.
/// </summary>
public sealed class CrsClient : GeoServerClientBase
{
    internal CrsClient(HttpClient httpClient) : base(httpClient)
    {
    }

    /// <summary>
    /// Lists CRS identifiers with optional filtering and paging.
    /// </summary>
    public Task<CrsListResponse> GetAllAsync(
        string? authority = null,
        string? query = null,
        int? offset = null,
        int? limit = null,
        CancellationToken cancellationToken = default)
        => SendAsync<CrsListResponse>(HttpMethod.Get, BuildListPath(authority, query, offset, limit), cancellationToken: cancellationToken);

    /// <summary>
    /// Lists CRS identifiers with optional filtering and paging (synchronous).
    /// </summary>
    public CrsListResponse GetAll(string? authority = null, string? query = null, int? offset = null, int? limit = null)
        => Send<CrsListResponse>(HttpMethod.Get, BuildListPath(authority, query, offset, limit));

    /// <summary>
    /// Gets a CRS definition as JSON.
    /// </summary>
    public Task<CrsDefinitionResponse> GetByIdentifierAsync(string identifier, CancellationToken cancellationToken = default)
        => SendAsync<CrsDefinitionResponse>(HttpMethod.Get, $"crs/{Encode(identifier)}.json", cancellationToken: cancellationToken);

    /// <summary>
    /// Gets a CRS definition as JSON (synchronous).
    /// </summary>
    public CrsDefinitionResponse GetByIdentifier(string identifier)
        => Send<CrsDefinitionResponse>(HttpMethod.Get, $"crs/{Encode(identifier)}.json");

    /// <summary>
    /// Gets a CRS definition as WKT text.
    /// </summary>
    public Task<string> GetWktByIdentifierAsync(string identifier, CancellationToken cancellationToken = default)
        => SendRawAsync(HttpMethod.Get, $"crs/{Encode(identifier)}.wkt", cancellationToken: cancellationToken);

    /// <summary>
    /// Gets a CRS definition as WKT text (synchronous).
    /// </summary>
    public string GetWktByIdentifier(string identifier)
        => SendRaw(HttpMethod.Get, $"crs/{Encode(identifier)}.wkt");

    /// <summary>
    /// Lists available CRS authorities.
    /// </summary>
    public Task<CrsAuthoritiesResponse> GetAuthoritiesAsync(CancellationToken cancellationToken = default)
        => SendAsync<CrsAuthoritiesResponse>(HttpMethod.Get, "crs/authorities.json", cancellationToken: cancellationToken);

    /// <summary>
    /// Lists available CRS authorities (synchronous).
    /// </summary>
    public CrsAuthoritiesResponse GetAuthorities()
        => Send<CrsAuthoritiesResponse>(HttpMethod.Get, "crs/authorities.json");

    private static string BuildListPath(string? authority, string? query, int? offset, int? limit)
    {
        var queryParameters = new List<string>();
        if (!string.IsNullOrWhiteSpace(authority))
        {
            queryParameters.Add($"authority={Uri.EscapeDataString(authority)}");
        }

        if (!string.IsNullOrWhiteSpace(query))
        {
            queryParameters.Add($"query={Uri.EscapeDataString(query)}");
        }

        if (offset.HasValue)
        {
            if (offset.Value < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(offset), "Offset must be greater than or equal to zero.");
            }

            queryParameters.Add($"offset={offset.Value}");
        }

        if (limit.HasValue)
        {
            if (limit.Value < 1 || limit.Value > 200)
            {
                throw new ArgumentOutOfRangeException(nameof(limit), "Limit must be between 1 and 200.");
            }

            queryParameters.Add($"limit={limit.Value}");
        }

        if (queryParameters.Count == 0)
        {
            return "crs.json";
        }

        var builder = new StringBuilder("crs.json?");
        for (var i = 0; i < queryParameters.Count; i++)
        {
            if (i > 0)
            {
                builder.Append('&');
            }

            builder.Append(queryParameters[i]);
        }

        return builder.ToString();
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
