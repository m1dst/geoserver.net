using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using geoserver.net.Models.Transforms;

namespace geoserver.net.Clients;

/// <summary>
/// Wrapper around WFS transforms endpoints.
/// </summary>
public sealed class TransformsClient : GeoServerClientBase
{
    private const string BasePath = "services/wfs/transforms";

    internal TransformsClient(HttpClient httpClient) : base(httpClient)
    {
    }

    /// <summary>
    /// Lists transforms.
    /// </summary>
    public Task<TransformsResponse> GetAllAsync(CancellationToken cancellationToken = default)
        => SendAsync<TransformsResponse>(HttpMethod.Get, $"{BasePath}.json", cancellationToken: cancellationToken);

    /// <summary>
    /// Lists transforms (synchronous).
    /// </summary>
    public TransformsResponse GetAll()
        => Send<TransformsResponse>(HttpMethod.Get, $"{BasePath}.json");

    /// <summary>
    /// Gets a transform definition.
    /// </summary>
    public Task<TransformsResponse> GetByNameAsync(string transformName, CancellationToken cancellationToken = default)
        => SendAsync<TransformsResponse>(HttpMethod.Get, $"{BasePath}/{Encode(transformName)}.json", cancellationToken: cancellationToken);

    /// <summary>
    /// Gets a transform definition (synchronous).
    /// </summary>
    public TransformsResponse GetByName(string transformName)
        => Send<TransformsResponse>(HttpMethod.Get, $"{BasePath}/{Encode(transformName)}.json");

    /// <summary>
    /// Creates a transform from JSON/XML payload.
    /// </summary>
    public Task CreateAsync(object transformPayload, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Post, BasePath, transformPayload, cancellationToken);

    /// <summary>
    /// Creates a transform from JSON/XML payload (synchronous).
    /// </summary>
    public void Create(object transformPayload)
        => Send(HttpMethod.Post, BasePath, transformPayload);

    /// <summary>
    /// Creates a transform by posting XSLT directly.
    /// </summary>
    public async Task CreateXsltAsync(string xsltContent, string query, CancellationToken cancellationToken = default)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, $"{BasePath}?{query.TrimStart('?')}")
        {
            Content = new StringContent(xsltContent ?? string.Empty, Encoding.UTF8, "application/xslt+xml")
        };

        using var response = await HttpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);
        var body = response.Content is null ? string.Empty : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        if (!response.IsSuccessStatusCode)
        {
            throw new GeoServerApiException(response.StatusCode, body, $"GeoServer REST call failed for '{request.RequestUri}'.");
        }
    }

    /// <summary>
    /// Creates a transform by posting XSLT directly (synchronous).
    /// </summary>
    public void CreateXslt(string xsltContent, string query)
        => CreateXsltAsync(xsltContent, query).GetAwaiter().GetResult();

    /// <summary>
    /// Updates a transform.
    /// </summary>
    public Task UpdateAsync(string transformName, object transformPayload, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Put, $"{BasePath}/{Encode(transformName)}", transformPayload, cancellationToken);

    /// <summary>
    /// Updates a transform (synchronous).
    /// </summary>
    public void Update(string transformName, object transformPayload)
        => Send(HttpMethod.Put, $"{BasePath}/{Encode(transformName)}", transformPayload);

    /// <summary>
    /// Deletes a transform.
    /// </summary>
    public Task DeleteAsync(string transformName, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Delete, $"{BasePath}/{Encode(transformName)}", cancellationToken: cancellationToken);

    /// <summary>
    /// Deletes a transform (synchronous).
    /// </summary>
    public void Delete(string transformName)
        => Send(HttpMethod.Delete, $"{BasePath}/{Encode(transformName)}");

    private static string Encode(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Value is required.", nameof(value));
        }

        return Uri.EscapeDataString(value);
    }
}
