using System;
using System.Net;

namespace geoserver.net;

/// <summary>
/// Represents a non-success response from GeoServer REST.
/// </summary>
public sealed class GeoServerApiException : Exception
{
    /// <summary>
    /// Creates a new API exception.
    /// </summary>
    public GeoServerApiException(HttpStatusCode statusCode, string responseBody, string message)
        : base(message)
    {
        StatusCode = statusCode;
        ResponseBody = responseBody;
    }

    /// <summary>
    /// HTTP status code from the server.
    /// </summary>
    public HttpStatusCode StatusCode { get; }

    /// <summary>
    /// Raw response body from the server.
    /// </summary>
    public string ResponseBody { get; }
}
