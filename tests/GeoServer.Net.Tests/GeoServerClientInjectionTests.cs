using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using geoserver.net;
using Xunit;

namespace GeoServer.Net.Tests;

/// <summary>
/// Represents the GeoServerClientInjectionTests type.
/// </summary>
public sealed class GeoServerClientInjectionTests
{
    /// <summary>
    /// Executes the DoesNotDisposeExternalHttpClientByDefault operation.
    /// </summary>
    [Fact]
    public void DoesNotDisposeExternalHttpClientByDefault()
    {
        var handler = new DisposableProbeHandler();
        var httpClient = new HttpClient(handler) { BaseAddress = new Uri("http://localhost:8080/geoserver/rest/") };

        using (var client = new GeoServerClient(httpClient))
        {
            _ = client;
        }

        Assert.False(handler.IsDisposed);
    }

    /// <summary>
    /// Executes the DisposesExternalHttpClientWhenRequested operation.
    /// </summary>
    [Fact]
    public void DisposesExternalHttpClientWhenRequested()
    {
        var handler = new DisposableProbeHandler();
        var httpClient = new HttpClient(handler) { BaseAddress = new Uri("http://localhost:8080/geoserver/rest/") };

        using (var client = new GeoServerClient(httpClient, disposeHttpClient: true))
        {
            _ = client;
        }

        Assert.True(handler.IsDisposed);
    }

    /// <summary>
    /// Executes the ExtensionMethodCreatesClient operation.
    /// </summary>
    [Fact]
    public void ExtensionMethodCreatesClient()
    {
        var handler = new TestHttpMessageHandler(_ => TestHttpMessageHandler.Json(@"{""workspaces"":{""workspace"":[]}}"));
        var httpClient = new HttpClient(handler) { BaseAddress = new Uri("http://localhost:8080/geoserver/rest/") };
        using var client = httpClient.CreateGeoServerClient();
        _ = client.Workspaces.GetAll();
        Assert.Single(handler.Requests);
    }

    private sealed class DisposableProbeHandler : HttpMessageHandler
    {
        /// <summary>
        /// Gets or sets the IsDisposed value.
        /// </summary>
        public bool IsDisposed { get; private set; }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            => Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(@"{""workspaces"":{""workspace"":[]}}")
            });

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                IsDisposed = true;
            }

            base.Dispose(disposing);
        }
    }
}
