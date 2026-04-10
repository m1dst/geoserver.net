using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
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

    /// <summary>
    /// Executes the ExtensionMethodWithOptions_AppliesOptionsToInjectedHttpClient operation.
    /// </summary>
    [Fact]
    public void ExtensionMethodWithOptions_AppliesOptionsToInjectedHttpClient()
    {
        var handler = new TestHttpMessageHandler(_ => TestHttpMessageHandler.Json(@"{""workspaces"":{""workspace"":[]}}"));
        var httpClient = new HttpClient(handler) { BaseAddress = new Uri("http://placeholder.local/base/") };
        var options = new GeoServerClientOptions
        {
            BaseUri = new Uri("http://localhost:8080/geoserver/rest"),
            Username = "admin",
            Password = "geoserver",
            Timeout = TimeSpan.FromSeconds(45)
        };

        using var client = httpClient.CreateGeoServerClient(options);
        _ = client.Workspaces.GetAll();

        Assert.Single(handler.Requests);
        Assert.Equal("http://localhost:8080/geoserver/rest/workspaces.json", handler.Requests[0].RequestUri!.AbsoluteUri);

        var expected = Convert.ToBase64String(Encoding.UTF8.GetBytes("admin:geoserver"));
        Assert.Equal(new AuthenticationHeaderValue("Basic", expected), handler.Requests[0].Headers.Authorization);
        Assert.Equal("http://placeholder.local/base/", httpClient.BaseAddress!.AbsoluteUri);
    }

    /// <summary>
    /// Executes the ConstructorWithOptions_DoesNotMutateInjectedHttpClientDefaults operation.
    /// </summary>
    [Fact]
    public void ConstructorWithOptions_DoesNotMutateInjectedHttpClientDefaults()
    {
        var handler = new TestHttpMessageHandler(_ => TestHttpMessageHandler.Json(@"{""workspaces"":{""workspace"":[]}}"));
        var httpClient = new HttpClient(handler)
        {
            BaseAddress = new Uri("http://unchanged.local/base/")
        };
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", "cHJldmlvdXM6YXV0aA==");

        var options = new GeoServerClientOptions
        {
            BaseUri = new Uri("http://localhost:8080/geoserver/rest"),
            Username = "admin",
            Password = "geoserver"
        };

        using var client = new GeoServerClient(httpClient, options);
        _ = client.Workspaces.GetAll();

        Assert.Single(handler.Requests);
        Assert.Equal("http://localhost:8080/geoserver/rest/workspaces.json", handler.Requests[0].RequestUri!.AbsoluteUri);
        var expected = Convert.ToBase64String(Encoding.UTF8.GetBytes("admin:geoserver"));
        Assert.Equal(new AuthenticationHeaderValue("Basic", expected), handler.Requests[0].Headers.Authorization);

        Assert.Equal("http://unchanged.local/base/", httpClient.BaseAddress!.AbsoluteUri);
        Assert.Equal(new AuthenticationHeaderValue("Basic", "cHJldmlvdXM6YXV0aA=="), httpClient.DefaultRequestHeaders.Authorization);
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
