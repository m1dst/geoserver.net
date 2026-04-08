using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace geoserver.net;

public abstract class GeoServerClientBase
{
    private readonly HttpClient _httpClient;

    protected GeoServerClientBase(HttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
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

    protected T Send<T>(HttpMethod method, string path, object? body = null)
        => SendAsync<T>(method, path, body).GetAwaiter().GetResult();

    protected void Send(HttpMethod method, string path, object? body = null)
        => SendAsync(method, path, body).GetAwaiter().GetResult();

    private static HttpRequestMessage BuildRequest(HttpMethod method, string path, object? body)
    {
        var request = new HttpRequestMessage(method, path);
        if (body is not null)
        {
            var json = JsonConvert.SerializeObject(body);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");
        }

        return request;
    }

    private static GeoServerApiException CreateApiException(HttpStatusCode statusCode, string path, string content)
        => new(statusCode, content, $"GeoServer REST call failed for '{path}' with status {(int)statusCode} ({statusCode}).");

    protected readonly struct NoContent
    {
        public static NoContent Value => default;
    }
}
