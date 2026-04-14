using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using GeoServer.Models.Security;

namespace GeoServer.Clients;

/// <summary>
/// Wrapper around GeoServer authentication providers endpoints.
/// </summary>
public sealed class AuthProvidersClient : GeoServerClientBase
{
    internal AuthProvidersClient(HttpClient httpClient, GeoServerRequestContext? requestContext = null) : base(httpClient, requestContext)
    {
    }

    /// <summary>
    /// Executes the GetAllAsync operation.
    /// </summary>
    public Task<AuthProvidersResponse> GetAllAsync(CancellationToken cancellationToken = default)
        => SendAsync<AuthProvidersResponse>(HttpMethod.Get, "security/authproviders", cancellationToken: cancellationToken);

    /// <summary>
    /// Executes the GetAll operation.
    /// </summary>
    public AuthProvidersResponse GetAll()
        => Send<AuthProvidersResponse>(HttpMethod.Get, "security/authproviders");

    /// <summary>
    /// Executes the GetByNameAsync operation.
    /// </summary>
    public Task<AuthProvidersResponse> GetByNameAsync(string providerName, CancellationToken cancellationToken = default)
        => SendAsync<AuthProvidersResponse>(HttpMethod.Get, $"security/authproviders/{Encode(providerName)}", cancellationToken: cancellationToken);

    /// <summary>
    /// Executes the GetByName operation.
    /// </summary>
    public AuthProvidersResponse GetByName(string providerName)
        => Send<AuthProvidersResponse>(HttpMethod.Get, $"security/authproviders/{Encode(providerName)}");

    /// <summary>
    /// Executes the CreateAsync operation.
    /// </summary>
    public Task CreateAsync(object providerPayload, int? position = null, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Post, BuildBasePath(position), providerPayload, cancellationToken);

    /// <summary>
    /// Executes the Create operation.
    /// </summary>
    public void Create(object providerPayload, int? position = null)
        => Send(HttpMethod.Post, BuildBasePath(position), providerPayload);

    /// <summary>
    /// Executes the UpdateAsync operation.
    /// </summary>
    public Task UpdateAsync(string providerName, object providerPayload, int? position = null, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Put, BuildByNamePath(providerName, position), providerPayload, cancellationToken);

    /// <summary>
    /// Executes the Update operation.
    /// </summary>
    public void Update(string providerName, object providerPayload, int? position = null)
        => Send(HttpMethod.Put, BuildByNamePath(providerName, position), providerPayload);

    /// <summary>
    /// Executes the DeleteAsync operation.
    /// </summary>
    public Task DeleteAsync(string providerName, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Delete, $"security/authproviders/{Encode(providerName)}", cancellationToken: cancellationToken);

    /// <summary>
    /// Executes the Delete operation.
    /// </summary>
    public void Delete(string providerName)
        => Send(HttpMethod.Delete, $"security/authproviders/{Encode(providerName)}");

    /// <summary>
    /// Executes the SetOrderAsync operation.
    /// </summary>
    public Task SetOrderAsync(AuthProvidersOrderRequest orderRequest, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Put, "security/authproviders/order", orderRequest, cancellationToken);

    /// <summary>
    /// Executes the SetOrder operation.
    /// </summary>
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
