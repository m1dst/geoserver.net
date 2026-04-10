using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using geoserver.net.Models.Security;

namespace geoserver.net.Clients;

/// <summary>
/// Wrapper around GeoServer security filter chain endpoints.
/// </summary>
public sealed class FilterChainsClient : GeoServerClientBase
{
    internal FilterChainsClient(HttpClient httpClient, GeoServerRequestContext? requestContext = null) : base(httpClient, requestContext)
    {
    }

    /// <summary>
    /// Lists configured filter chains.
    /// </summary>
    public Task<FilterChainsResponse> GetAllAsync(CancellationToken cancellationToken = default)
        => SendAsync<FilterChainsResponse>(HttpMethod.Get, "security/filterchain", cancellationToken: cancellationToken);

    /// <summary>
    /// Lists configured filter chains (synchronous).
    /// </summary>
    public FilterChainsResponse GetAll()
        => Send<FilterChainsResponse>(HttpMethod.Get, "security/filterchain");

    /// <summary>
    /// Gets a filter chain by name.
    /// </summary>
    public Task<FilterChainsResponse> GetByNameAsync(string chainName, CancellationToken cancellationToken = default)
        => SendAsync<FilterChainsResponse>(HttpMethod.Get, $"security/filterchain/{Encode(chainName)}", cancellationToken: cancellationToken);

    /// <summary>
    /// Gets a filter chain by name (synchronous).
    /// </summary>
    public FilterChainsResponse GetByName(string chainName)
        => Send<FilterChainsResponse>(HttpMethod.Get, $"security/filterchain/{Encode(chainName)}");

    /// <summary>
    /// Creates a new filter chain.
    /// </summary>
    public Task CreateAsync(object filterChainPayload, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Post, "security/filterchain", filterChainPayload, cancellationToken);

    /// <summary>
    /// Creates a new filter chain (synchronous).
    /// </summary>
    public void Create(object filterChainPayload)
        => Send(HttpMethod.Post, "security/filterchain", filterChainPayload);

    /// <summary>
    /// Updates an existing filter chain.
    /// </summary>
    public Task UpdateAsync(string chainName, object filterChainPayload, int? position = null, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Put, BuildChainPath(chainName, position), filterChainPayload, cancellationToken);

    /// <summary>
    /// Updates an existing filter chain (synchronous).
    /// </summary>
    public void Update(string chainName, object filterChainPayload, int? position = null)
        => Send(HttpMethod.Put, BuildChainPath(chainName, position), filterChainPayload);

    /// <summary>
    /// Deletes a filter chain.
    /// </summary>
    public Task DeleteAsync(string chainName, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Delete, $"security/filterchain/{Encode(chainName)}", cancellationToken: cancellationToken);

    /// <summary>
    /// Deletes a filter chain (synchronous).
    /// </summary>
    public void Delete(string chainName)
        => Send(HttpMethod.Delete, $"security/filterchain/{Encode(chainName)}");

    /// <summary>
    /// Reorders filter chains.
    /// </summary>
    public Task SetOrderAsync(FilterChainsOrderRequest order, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Put, "security/filterchain/order", order, cancellationToken);

    /// <summary>
    /// Reorders filter chains (synchronous).
    /// </summary>
    public void SetOrder(FilterChainsOrderRequest order)
        => Send(HttpMethod.Put, "security/filterchain/order", order);

    private static string BuildChainPath(string chainName, int? position)
    {
        var path = $"security/filterchain/{Encode(chainName)}";
        if (!position.HasValue)
        {
            return path;
        }

        if (position.Value < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(position), "Position must be greater than or equal to zero.");
        }

        return $"{path}?position={position.Value}";
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
