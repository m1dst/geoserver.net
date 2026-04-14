using System;
using System.Net.Http;
using GeoServer;

namespace GeoServer.Tests;

internal static class GeoServerClientFactory
{
    /// <summary>
    /// Executes the Create operation.
    /// </summary>
    public static (GeoServerClient Client, TestHttpMessageHandler Handler) Create(Func<HttpRequestMessage, HttpResponseMessage> responder)
    {
        var handler = new TestHttpMessageHandler(responder);
        var httpClient = new HttpClient(handler)
        {
            BaseAddress = new Uri("http://localhost:8080/geoserver/rest/")
        };

        return (new GeoServerClient(httpClient, disposeHttpClient: true), handler);
    }
}
