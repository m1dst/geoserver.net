using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using geoserver.net.Models.ProxyBaseExtension;

namespace geoserver.net.Clients;

/// <summary>
/// Wrapper around Proxy Base Extension rule endpoints.
/// </summary>
public sealed class ProxyBaseExtensionClient : GeoServerClientBase
{
    internal ProxyBaseExtensionClient(HttpClient httpClient, GeoServerRequestContext? requestContext = null) : base(httpClient, requestContext)
    {
    }

    /// <summary>
    /// Lists proxy base extension rules.
    /// </summary>
    public Task<ProxyBaseExtensionRuleListResponse> GetAllAsync(CancellationToken cancellationToken = default)
        => SendAsync<ProxyBaseExtensionRuleListResponse>(HttpMethod.Get, "proxy-base-ext.json", cancellationToken: cancellationToken);

    /// <summary>
    /// Lists proxy base extension rules (synchronous).
    /// </summary>
    public ProxyBaseExtensionRuleListResponse GetAll()
        => Send<ProxyBaseExtensionRuleListResponse>(HttpMethod.Get, "proxy-base-ext.json");

    /// <summary>
    /// Gets one proxy base extension rule by id.
    /// </summary>
    public Task<ProxyBaseExtensionRuleResponse> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        => SendAsync<ProxyBaseExtensionRuleResponse>(HttpMethod.Get, $"proxy-base-ext/rules/{Encode(id)}.json", cancellationToken: cancellationToken);

    /// <summary>
    /// Gets one proxy base extension rule by id (synchronous).
    /// </summary>
    public ProxyBaseExtensionRuleResponse GetById(string id)
        => Send<ProxyBaseExtensionRuleResponse>(HttpMethod.Get, $"proxy-base-ext/rules/{Encode(id)}.json");

    /// <summary>
    /// Creates a proxy base extension rule.
    /// </summary>
    public Task CreateAsync(object rulePayload, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Post, "proxy-base-ext", rulePayload, cancellationToken);

    /// <summary>
    /// Creates a proxy base extension rule (synchronous).
    /// </summary>
    public void Create(object rulePayload)
        => Send(HttpMethod.Post, "proxy-base-ext", rulePayload);

    /// <summary>
    /// Updates a proxy base extension rule by id.
    /// </summary>
    public Task UpdateAsync(string id, object rulePayload, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Put, $"proxy-base-ext/rules/{Encode(id)}", rulePayload, cancellationToken);

    /// <summary>
    /// Updates a proxy base extension rule by id (synchronous).
    /// </summary>
    public void Update(string id, object rulePayload)
        => Send(HttpMethod.Put, $"proxy-base-ext/rules/{Encode(id)}", rulePayload);

    /// <summary>
    /// Deletes a proxy base extension rule by id.
    /// </summary>
    public Task DeleteAsync(string id, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Delete, $"proxy-base-ext/rules/{Encode(id)}", cancellationToken: cancellationToken);

    /// <summary>
    /// Deletes a proxy base extension rule by id (synchronous).
    /// </summary>
    public void Delete(string id)
        => Send(HttpMethod.Delete, $"proxy-base-ext/rules/{Encode(id)}");

    private static string Encode(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Value is required.", nameof(value));
        }

        return Uri.EscapeDataString(value);
    }
}
