using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace geoserver.net;

/// <summary>
/// Represents the GeoServerClientBase type.
/// </summary>
public abstract class GeoServerClientBase
{
    private readonly HttpClient _httpClient;
    private readonly geoserver.net.Clients.GeoServerRequestContext? _requestContext;
    protected HttpClient HttpClient => _httpClient;

    private protected GeoServerClientBase(HttpClient httpClient, geoserver.net.Clients.GeoServerRequestContext? requestContext = null)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _requestContext = requestContext;
    }

    protected async Task<T> SendAsync<T>(HttpMethod method, string path, object? body = null, CancellationToken cancellationToken = default)
    {
        var request = BuildRequest(method, path, body);
        using var response = await _httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);
        var content = response.Content is null
            ? string.Empty
            : await response.Content.ReadAsStringAsync().ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
        {
            throw CreateApiException(response.StatusCode, path, content);
        }

        if (typeof(T) == typeof(NoContent))
        {
            return (T)(object)NoContent.Value;
        }

        if (string.IsNullOrWhiteSpace(content))
        {
            throw new GeoServerApiException(response.StatusCode, content, $"Expected JSON response for '{path}' but body was empty.");
        }

        var result = JsonConvert.DeserializeObject<T>(content);
        if (result is null)
        {
            throw new GeoServerApiException(response.StatusCode, content, $"Unable to deserialize response for '{path}' as {typeof(T).Name}.");
        }

        return result;
    }

    protected Task SendAsync(HttpMethod method, string path, object? body = null, CancellationToken cancellationToken = default)
        => SendAsync<NoContent>(method, path, body, cancellationToken);

    protected async Task<string> SendRawAsync(HttpMethod method, string path, object? body = null, CancellationToken cancellationToken = default)
    {
        var request = BuildRequest(method, path, body);
        request.Headers.Accept.Clear();
        request.Headers.Accept.ParseAdd("*/*");
        using var response = await _httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);
        var content = response.Content is null
            ? string.Empty
            : await response.Content.ReadAsStringAsync().ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
        {
            throw CreateApiException(response.StatusCode, path, content);
        }

        return content;
    }

    protected T Send<T>(HttpMethod method, string path, object? body = null)
        => SendAsync<T>(method, path, body).GetAwaiter().GetResult();

    protected void Send(HttpMethod method, string path, object? body = null)
        => SendAsync(method, path, body).GetAwaiter().GetResult();

    protected string SendRaw(HttpMethod method, string path, object? body = null)
        => SendRawAsync(method, path, body).GetAwaiter().GetResult();

    private HttpRequestMessage BuildRequest(HttpMethod method, string path, object? body)
    {
        var request = new HttpRequestMessage(method, BuildRequestUri(path));
        if (_requestContext?.Authorization is not null)
        {
            request.Headers.Authorization = new AuthenticationHeaderValue(
                _requestContext.Authorization.Scheme,
                _requestContext.Authorization.Parameter);
        }

        if (body is not null)
        {
            var json = JsonConvert.SerializeObject(body);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");
        }

        return request;
    }

    private Uri BuildRequestUri(string path)
    {
        if (_requestContext is null || _requestContext.BaseUri is null)
        {
            return new Uri(path, UriKind.RelativeOrAbsolute);
        }

        var relativePath = path.TrimStart('/');
        return new Uri(_requestContext.BaseUri, relativePath);
    }

    private static GeoServerApiException CreateApiException(HttpStatusCode statusCode, string path, string content)
        => new(statusCode, content, $"GeoServer REST call failed for '{path}' with status {(int)statusCode} ({statusCode}).");

    protected readonly struct NoContent
    {
        public static NoContent Value => default;
    }
}
