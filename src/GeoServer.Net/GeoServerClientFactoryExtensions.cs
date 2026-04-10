using System;
using System.Net.Http;

namespace geoserver.net;

/// <summary>
/// Helpers for constructing <see cref="GeoServerClient"/> with externally managed <see cref="HttpClient"/>.
/// </summary>
public static class GeoServerClientFactoryExtensions
{
    /// <summary>
    /// Creates a new <see cref="GeoServerClient"/> from a caller-managed <see cref="HttpClient"/>.
    /// </summary>
    /// <param name="httpClient">Reusable client (for example from IHttpClientFactory).</param>
    /// <param name="disposeHttpClient">True to dispose the provided instance when the wrapper is disposed.</param>
    public static GeoServerClient CreateGeoServerClient(this HttpClient httpClient, bool disposeHttpClient = false)
        => new(httpClient ?? throw new ArgumentNullException(nameof(httpClient)), disposeHttpClient);

    /// <summary>
    /// Creates a <see cref="GeoServerClient"/> from an existing <see cref="HttpClient"/> and applies options.
    /// </summary>
    /// <param name="httpClient">Reusable client (for example from IHttpClientFactory).</param>
    /// <param name="options">GeoServer options to apply (base URI, timeout, auth headers).</param>
    /// <param name="disposeHttpClient">True to dispose the provided instance when the wrapper is disposed.</param>
    public static GeoServerClient CreateGeoServerClient(
        this HttpClient httpClient,
        GeoServerClientOptions options,
        bool disposeHttpClient = false)
    {
        if (httpClient is null)
        {
            throw new ArgumentNullException(nameof(httpClient));
        }

        return new GeoServerClient(httpClient, options, disposeHttpClient);
    }
}
