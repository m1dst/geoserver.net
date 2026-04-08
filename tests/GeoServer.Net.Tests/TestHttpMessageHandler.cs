using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace GeoServer.Net.Tests;

internal sealed class TestHttpMessageHandler : HttpMessageHandler
{
    private readonly Func<HttpRequestMessage, HttpResponseMessage> _responder;

    public TestHttpMessageHandler(Func<HttpRequestMessage, HttpResponseMessage> responder)
    {
        _responder = responder;
    }

    public List<HttpRequestMessage> Requests { get; } = new();

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        Requests.Add(request);
        return Task.FromResult(_responder(request));
    }

    public static HttpResponseMessage Json(string json, HttpStatusCode statusCode = HttpStatusCode.OK)
        => new(statusCode)
        {
            Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json")
        };

    public static HttpResponseMessage NoContent(HttpStatusCode statusCode = HttpStatusCode.OK)
        => new(statusCode);
}
