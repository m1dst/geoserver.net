using Newtonsoft.Json;

namespace geoserver.net.Models.Stores;

/// <summary>
/// Request payload used to create a coverage store.
/// </summary>
public sealed class CoverageStoreCreateRequest
{
    /// <summary>
    /// Coverage store name.
    /// </summary>
    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Coverage store type.
    /// </summary>
    [JsonProperty("type")]
    public string Type { get; set; } = "GeoTIFF";

    /// <summary>
    /// Optional URL to underlying resource.
    /// </summary>
    [JsonProperty("url")]
    public string? Url { get; set; }
}
