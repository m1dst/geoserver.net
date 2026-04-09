using Newtonsoft.Json;

namespace geoserver.net.Models.UrlChecks;

/// <summary>
/// Response wrapper for a single URL external access check.
/// </summary>
public sealed class UrlCheckResponse
{
    /// <summary>
    /// URL check payload.
    /// </summary>
    [JsonProperty("urlCheck")]
    public object? UrlCheck { get; set; }
}
