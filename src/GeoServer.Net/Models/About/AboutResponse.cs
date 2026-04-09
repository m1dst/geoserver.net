using Newtonsoft.Json;

namespace geoserver.net.Models.About;

/// <summary>
/// Generic response wrapper for GeoServer about endpoints.
/// </summary>
public sealed class AboutResponse
{
    /// <summary>
    /// About payload.
    /// </summary>
    [JsonProperty("about")]
    public object? About { get; set; }
}
