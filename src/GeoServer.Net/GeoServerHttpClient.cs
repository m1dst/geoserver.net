using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using geoserver.net.Clients;

namespace geoserver.net;

internal static class GeoServerHttpClient
{
    /// <summary>
    /// Executes the Create operation.
    /// </summary>
    public static HttpClient Create(GeoServerClientOptions options)
    {
        if (options is null)
        {
            throw new ArgumentNullException(nameof(options));
        }

        if (options.BaseUri is null)
        {
            throw new ArgumentException("BaseUri is required.", nameof(options));
        }

        var requestContext = CreateRequestContext(options);
        var httpClient = new HttpClient
        {
            BaseAddress = requestContext.BaseUri,
            Timeout = options.Timeout
        };

        var auth = requestContext.Authorization;
        if (auth is not null)
        {
            httpClient.DefaultRequestHeaders.Authorization = auth;
        }

        httpClient.DefaultRequestHeaders.Accept.Clear();
        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        return httpClient;
    }

    /// <summary>
    /// Applies <see cref="GeoServerClientOptions"/> to an existing client.
    /// </summary>
    /// <param name="httpClient">Existing reusable HTTP client.</param>
    /// <param name="options">GeoServer client options.</param>
    public static void ApplyOptions(HttpClient httpClient, GeoServerClientOptions options)
    {
        if (httpClient is null)
        {
            throw new ArgumentNullException(nameof(httpClient));
        }

        if (options is null)
        {
            throw new ArgumentNullException(nameof(options));
        }

        if (options.BaseUri is null)
        {
            throw new ArgumentException("BaseUri is required.", nameof(options));
        }

        httpClient.BaseAddress = EnsureTrailingSlash(options.BaseUri);
        httpClient.Timeout = options.Timeout;

        var authBytes = Encoding.UTF8.GetBytes($"{options.Username}:{options.Password}");
        httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authBytes));
        httpClient.DefaultRequestHeaders.Accept.Clear();
        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public static GeoServerRequestContext CreateRequestContext(GeoServerClientOptions options)
    {
        if (options is null)
        {
            throw new ArgumentNullException(nameof(options));
        }

        if (options.BaseUri is null)
        {
            throw new ArgumentException("BaseUri is required.", nameof(options));
        }

        var username = options.Username ?? string.Empty;
        var password = options.Password ?? string.Empty;
        var authBytes = Encoding.UTF8.GetBytes($"{username}:{password}");
        var authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authBytes));

        return new GeoServerRequestContext
        {
            BaseUri = EnsureTrailingSlash(options.BaseUri),
            Authorization = authorization
        };
    }

    private static Uri EnsureTrailingSlash(Uri uri)
    {
        var text = uri.ToString();
        if (!text.EndsWith("/", StringComparison.Ordinal))
        {
            text += "/";
        }

        return new Uri(text, UriKind.Absolute);
    }
}
