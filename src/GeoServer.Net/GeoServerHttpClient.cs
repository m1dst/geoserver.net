using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

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

        var httpClient = new HttpClient
        {
            BaseAddress = EnsureTrailingSlash(options.BaseUri),
            Timeout = options.Timeout
        };

        var authBytes = Encoding.UTF8.GetBytes($"{options.Username}:{options.Password}");
        httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authBytes));
        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        return httpClient;
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
