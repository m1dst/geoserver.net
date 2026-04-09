using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using geoserver.net.Models.Security;

namespace geoserver.net.Clients;

/// <summary>
/// Wrapper around GeoServer authentication filters endpoints.
/// </summary>
public sealed class AuthFiltersClient : GeoServerClientBase
{
    internal AuthFiltersClient(HttpClient httpClient) : base(httpClient)
    {
    }

    public Task<AuthFiltersResponse> GetAllAsync(CancellationToken cancellationToken = default)
        => SendAsync<AuthFiltersResponse>(HttpMethod.Get, "security/authfilters", cancellationToken: cancellationToken);

    public AuthFiltersResponse GetAll()
        => Send<AuthFiltersResponse>(HttpMethod.Get, "security/authfilters");

    public Task<AuthFiltersResponse> GetByNameAsync(string filterName, CancellationToken cancellationToken = default)
        => SendAsync<AuthFiltersResponse>(HttpMethod.Get, $"security/authfilters/{Encode(filterName)}", cancellationToken: cancellationToken);

    public AuthFiltersResponse GetByName(string filterName)
        => Send<AuthFiltersResponse>(HttpMethod.Get, $"security/authfilters/{Encode(filterName)}");

    public Task CreateAsync(object filterPayload, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Post, "security/authfilters", filterPayload, cancellationToken);

    public void Create(object filterPayload)
        => Send(HttpMethod.Post, "security/authfilters", filterPayload);

    public Task UpdateAsync(string filterName, object filterPayload, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Put, $"security/authfilters/{Encode(filterName)}", filterPayload, cancellationToken);

    public void Update(string filterName, object filterPayload)
        => Send(HttpMethod.Put, $"security/authfilters/{Encode(filterName)}", filterPayload);

    public Task DeleteAsync(string filterName, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Delete, $"security/authfilters/{Encode(filterName)}", cancellationToken: cancellationToken);

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
