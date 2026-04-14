using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace GeoServer.Tests;

internal sealed class TestHttpMessageHandler : HttpMessageHandler
{
    private readonly Func<HttpRequestMessage, HttpResponseMessage> _responder;

    public TestHttpMessageHandler(Func<HttpRequestMessage, HttpResponseMessage> responder)
    {
        _responder = responder;
    }

    /// <summary>
    /// Gets or sets the Requests value.
    /// </summary>
    public List<HttpRequestMessage> Requests { get; } = new();

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        Requests.Add(request);
        return Task.FromResult(_responder(request));
    }

    /// <summary>
    /// Executes the Json operation.
    /// </summary>
    public static HttpResponseMessage Json(string json, HttpStatusCode statusCode = HttpStatusCode.OK)
        => new(statusCode)
        {
            Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json")
        };

    /// <summary>
    /// Executes the NoContent operation.
    /// </summary>
    public static HttpResponseMessage NoContent(HttpStatusCode statusCode = HttpStatusCode.OK)
        => new(statusCode);
}
