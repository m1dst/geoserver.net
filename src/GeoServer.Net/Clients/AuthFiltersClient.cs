using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using GeoServer.Models.Security;

namespace GeoServer.Clients;

/// <summary>
/// Wrapper around GeoServer authentication filters endpoints.
/// </summary>
public sealed class AuthFiltersClient : GeoServerClientBase
{
    internal AuthFiltersClient(HttpClient httpClient, GeoServerRequestContext? requestContext = null) : base(httpClient, requestContext)
    {
    }

    /// <summary>
    /// Executes the GetAllAsync operation.
    /// </summary>
    public Task<AuthFiltersResponse> GetAllAsync(CancellationToken cancellationToken = default)
        => SendAsync<AuthFiltersResponse>(HttpMethod.Get, "security/authfilters", cancellationToken: cancellationToken);

    /// <summary>
    /// Executes the GetAll operation.
    /// </summary>
    public AuthFiltersResponse GetAll()
        => Send<AuthFiltersResponse>(HttpMethod.Get, "security/authfilters");

    /// <summary>
    /// Executes the GetByNameAsync operation.
    /// </summary>
    public Task<AuthFiltersResponse> GetByNameAsync(string filterName, CancellationToken cancellationToken = default)
        => SendAsync<AuthFiltersResponse>(HttpMethod.Get, $"security/authfilters/{Encode(filterName)}", cancellationToken: cancellationToken);

    /// <summary>
    /// Executes the GetByName operation.
    /// </summary>
    public AuthFiltersResponse GetByName(string filterName)
        => Send<AuthFiltersResponse>(HttpMethod.Get, $"security/authfilters/{Encode(filterName)}");

    /// <summary>
    /// Executes the CreateAsync operation.
    /// </summary>
    public Task CreateAsync(object filterPayload, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Post, "security/authfilters", filterPayload, cancellationToken);

    /// <summary>
    /// Executes the Create operation.
    /// </summary>
    public void Create(object filterPayload)
        => Send(HttpMethod.Post, "security/authfilters", filterPayload);

    /// <summary>
    /// Executes the UpdateAsync operation.
    /// </summary>
    public Task UpdateAsync(string filterName, object filterPayload, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Put, $"security/authfilters/{Encode(filterName)}", filterPayload, cancellationToken);

    /// <summary>
    /// Executes the Update operation.
    /// </summary>
    public void Update(string filterName, object filterPayload)
        => Send(HttpMethod.Put, $"security/authfilters/{Encode(filterName)}", filterPayload);

    /// <summary>
    /// Executes the DeleteAsync operation.
    /// </summary>
    public Task DeleteAsync(string filterName, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Delete, $"security/authfilters/{Encode(filterName)}", cancellationToken: cancellationToken);

    /// <summary>
    /// Executes the Delete operation.
    /// </summary>
    public void Delete(string filterName)
        => Send(HttpMethod.Delete, $"security/authfilters/{Encode(filterName)}");

    private static string Encode(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Value is required.", nameof(value));
        }

        return Uri.EscapeDataString(value);
    }
}
