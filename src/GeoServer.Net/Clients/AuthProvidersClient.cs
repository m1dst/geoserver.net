using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using geoserver.net.Models.Security;

namespace geoserver.net.Clients;

/// <summary>
/// Wrapper around GeoServer authentication providers endpoints.
/// </summary>
public sealed class AuthProvidersClient : GeoServerClientBase
{
    internal AuthProvidersClient(HttpClient httpClient) : base(httpClient)
    {
    }

    public Task<AuthProvidersResponse> GetAllAsync(CancellationToken cancellationToken = default)
        => SendAsync<AuthProvidersResponse>(HttpMethod.Get, "security/authproviders", cancellationToken: cancellationToken);

    public AuthProvidersResponse GetAll()
        => Send<AuthProvidersResponse>(HttpMethod.Get, "security/authproviders");

    public Task<AuthProvidersResponse> GetByNameAsync(string providerName, CancellationToken cancellationToken = default)
        => SendAsync<AuthProvidersResponse>(HttpMethod.Get, $"security/authproviders/{Encode(providerName)}", cancellationToken: cancellationToken);

    public AuthProvidersResponse GetByName(string providerName)
        => Send<AuthProvidersResponse>(HttpMethod.Get, $"security/authproviders/{Encode(providerName)}");

    public Task CreateAsync(object providerPayload, int? position = null, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Post, BuildBasePath(position), providerPayload, cancellationToken);

    public void Create(object providerPayload, int? position = null)
        => Send(HttpMethod.Post, BuildBasePath(position), providerPayload);

    public Task UpdateAsync(string providerName, object providerPayload, int? position = null, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Put, BuildByNamePath(providerName, position), providerPayload, cancellationToken);

    public void Update(string providerName, object providerPayload, int? position = null)
        => Send(HttpMethod.Put, BuildByNamePath(providerName, position), providerPayload);

    public Task DeleteAsync(string providerName, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Delete, $"security/authproviders/{Encode(providerName)}", cancellationToken: cancellationToken);

    public void Delete(string providerName)
        => Send(HttpMethod.Delete, $"security/authproviders/{Encode(providerName)}");

    public Task SetOrderAsync(AuthProvidersOrderRequest orderRequest, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Put, "security/authproviders/order", orderRequest, cancellationToken);

    public void SetOrder(AuthProvidersOrderRequest orderRequest)
        => Send(HttpMethod.Put, "security/authproviders/order", orderRequest);

    private static string BuildBasePath(int? position)
        => position.HasValue
            ? $"security/authproviders?position={position.Value.ToString(System.Globalization.CultureInfo.InvariantCulture)}"
            : "security/authproviders";

    private static string BuildByNamePath(string providerName, int? position)
    {
        var path = $"security/authproviders/{Encode(providerName)}";
        if (position.HasValue)
        {
            path += $"?position={position.Value.ToString(System.Globalization.CultureInfo.InvariantCulture)}";
        }

        return path;
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
