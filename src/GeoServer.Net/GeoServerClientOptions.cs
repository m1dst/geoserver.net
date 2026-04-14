using System;

namespace GeoServer;

/// <summary>
/// Configuration values for <see cref="GeoServerClient"/>.
/// </summary>
public sealed class GeoServerClientOptions
{
    /// <summary>
    /// Base GeoServer REST URL. Example: <c>http://localhost:8080/geoserver/rest</c>.
    /// </summary>
    public Uri BaseUri { get; set; } = default!;

    /// <summary>
    /// Username for basic authentication.
    /// </summary>
    public string Username { get; set; } = "admin";

    /// <summary>
    /// Password for basic authentication.
    /// </summary>
    public string Password { get; set; } = "geoserver";

    /// <summary>
    /// Optional request timeout. Defaults to 100 seconds.
    /// </summary>
    public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(100);
}
