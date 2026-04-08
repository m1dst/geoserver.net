using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using geoserver.net.Models.Styles;

namespace geoserver.net.Clients;

/// <summary>
/// Wrapper around GeoServer styles REST endpoints.
/// </summary>
public sealed class StylesClient : GeoServerClientBase
{
    private readonly HttpClient _httpClient;

    internal StylesClient(HttpClient httpClient) : base(httpClient)
    {
        _httpClient = httpClient;
    }

    /// <summary>
    /// Gets all styles.
    /// </summary>
    public Task<StyleListResponse> GetAllAsync(CancellationToken cancellationToken = default)
        => SendAsync<StyleListResponse>(HttpMethod.Get, "styles.json", cancellationToken: cancellationToken);

    /// <summary>
    /// Gets all styles (synchronous).
    /// </summary>
    public StyleListResponse GetAll() => Send<StyleListResponse>(HttpMethod.Get, "styles.json");

    /// <summary>
    /// Gets one style by name.
    /// </summary>
    public Task<StyleResponse> GetByNameAsync(string styleName, CancellationToken cancellationToken = default)
        => SendAsync<StyleResponse>(HttpMethod.Get, $"styles/{Encode(styleName)}.json", cancellationToken: cancellationToken);

    /// <summary>
    /// Gets one style by name (synchronous).
    /// </summary>
    public StyleResponse GetByName(string styleName)
        => Send<StyleResponse>(HttpMethod.Get, $"styles/{Encode(styleName)}.json");

    /// <summary>
    /// Creates style metadata entry.
    /// </summary>
    public Task CreateAsync(StyleCreateRequest request, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Post, "styles", new { style = request }, cancellationToken);

    /// <summary>
    /// Creates style metadata entry (synchronous).
    /// </summary>
    public void Create(StyleCreateRequest request)
        => Send(HttpMethod.Post, "styles", new { style = request });

    /// <summary>
    /// Uploads SLD content for an existing style.
    /// </summary>
    public async Task UploadSldAsync(string styleName, string sldContent, CancellationToken cancellationToken = default)
    {
        var request = new HttpRequestMessage(HttpMethod.Put, $"styles/{Encode(styleName)}")
        {
            Content = new StringContent(sldContent, Encoding.UTF8, "application/vnd.ogc.sld+xml")
        };

        using var response = await _httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);
        var body = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        if (!response.IsSuccessStatusCode)
        {
            throw new GeoServerApiException(response.StatusCode, body, $"GeoServer REST call failed for 'styles/{styleName}'.");
        }
    }

    /// <summary>
    /// Uploads SLD content for an existing style (synchronous).
    /// </summary>
    public void UploadSld(string styleName, string sldContent)
        => UploadSldAsync(styleName, sldContent).GetAwaiter().GetResult();

    /// <summary>
    /// Deletes a style.
    /// </summary>
    public Task DeleteAsync(string styleName, bool recurse = false, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Delete, $"styles/{Encode(styleName)}?recurse={recurse.ToString().ToLowerInvariant()}", cancellationToken: cancellationToken);

    /// <summary>
    /// Deletes a style (synchronous).
    /// </summary>
    public void Delete(string styleName, bool recurse = false)
        => Send(HttpMethod.Delete, $"styles/{Encode(styleName)}?recurse={recurse.ToString().ToLowerInvariant()}");

    private static string Encode(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Value is required.", nameof(value));
        }

        return Uri.EscapeDataString(value);
    }
}
