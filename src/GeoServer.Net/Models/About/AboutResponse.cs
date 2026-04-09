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

    /// <summary>
    /// Status payload used by some GeoServer versions for <c>about/status.json</c>.
    /// </summary>
    [JsonProperty("statuss")]
    public object? Statuses { get; set; }
}
