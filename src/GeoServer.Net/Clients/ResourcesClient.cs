using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using geoserver.net.Models.Resources;

namespace geoserver.net.Clients;

/// <summary>
/// Wrapper around generic GeoServer data directory resource endpoints.
/// </summary>
public sealed class ResourcesClient : GeoServerClientBase
{
    internal ResourcesClient(HttpClient httpClient) : base(httpClient)
    {
    }

    /// <summary>
    /// Gets a resource's content or directory listing in raw format.
    /// </summary>
    public Task<string> GetRawAsync(string pathToResource = "", string? query = null, CancellationToken cancellationToken = default)
        => SendRawAsync(HttpMethod.Get, BuildPath(pathToResource, query), cancellationToken: cancellationToken);

    /// <summary>
    /// Gets a resource's content or directory listing in raw format (synchronous).
    /// </summary>
    public string GetRaw(string pathToResource = "", string? query = null)
        => SendRaw(HttpMethod.Get, BuildPath(pathToResource, query));

    /// <summary>
    /// Gets resource metadata (JSON format).
    /// </summary>
    public Task<ResourceMetadataResponse> GetMetadataAsync(string pathToResource = "", CancellationToken cancellationToken = default)
        => SendAsync<ResourceMetadataResponse>(HttpMethod.Get, BuildPath(pathToResource, "operation=metadata&format=json"), cancellationToken: cancellationToken);

    /// <summary>
    /// Gets resource metadata (JSON format, synchronous).
    /// </summary>
    public ResourceMetadataResponse GetMetadata(string pathToResource = "")
        => Send<ResourceMetadataResponse>(HttpMethod.Get, BuildPath(pathToResource, "operation=metadata&format=json"));

    /// <summary>
    /// Retrieves resource metadata headers using HEAD.
    /// </summary>
    public Task HeadAsync(string pathToResource = "", CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Head, BuildPath(pathToResource), cancellationToken: cancellationToken);

    /// <summary>
    /// Retrieves resource metadata headers using HEAD (synchronous).
    /// </summary>
    public void Head(string pathToResource = "")
        => Send(HttpMethod.Head, BuildPath(pathToResource));

    /// <summary>
    /// Uploads or updates resource content.
    /// </summary>
    public async Task PutAsync(
        string pathToResource,
        string resourceBody,
        string mediaType = "text/plain",
        string? query = null,
        CancellationToken cancellationToken = default)
    {
        var request = new HttpRequestMessage(HttpMethod.Put, BuildPath(pathToResource, query))
        {
            Content = new StringContent(resourceBody ?? string.Empty, Encoding.UTF8, mediaType)
        };

        using var response = await HttpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);
        var content = response.Content is null
            ? string.Empty
            : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        if (!response.IsSuccessStatusCode)
        {
            throw new GeoServerApiException(response.StatusCode, content, $"GeoServer REST call failed for '{request.RequestUri}'.");
        }
    }

    /// <summary>
    /// Uploads or updates resource content (synchronous).
    /// </summary>
    public void Put(string pathToResource, string resourceBody, string mediaType = "text/plain", string? query = null)
        => PutAsync(pathToResource, resourceBody, mediaType, query).GetAwaiter().GetResult();

    /// <summary>
    /// Deletes a resource (recursively when target is a directory).
    /// </summary>
    public Task DeleteAsync(string pathToResource, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Delete, BuildPath(pathToResource), cancellationToken: cancellationToken);

    /// <summary>
    /// Deletes a resource (synchronous).
    /// </summary>
    public void Delete(string pathToResource)
        => Send(HttpMethod.Delete, BuildPath(pathToResource));

    private static string BuildPath(string? pathToResource, string? query = null)
    {
        var path = string.IsNullOrWhiteSpace(pathToResource)
            ? "resource"
            : $"resource/{EncodePath(pathToResource)}";

        if (string.IsNullOrWhiteSpace(query))
        {
            return path;
        }

        return $"{path}?{query.TrimStart('?')}";
    }

    private static string EncodePath(string value)
    {
        var trimmed = value.Trim('/');
        if (string.IsNullOrWhiteSpace(trimmed))
        {
            return string.Empty;
        }

        var segments = trimmed.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
        for (var i = 0; i < segments.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(segments[i]))
            {
                throw new ArgumentException("Resource path contains an empty segment.", nameof(value));
            }

            segments[i] = Uri.EscapeDataString(segments[i]);
        }

        return string.Join("/", segments);
    }
}
