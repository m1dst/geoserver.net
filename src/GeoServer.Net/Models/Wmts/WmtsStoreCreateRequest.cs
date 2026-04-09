using Newtonsoft.Json;

namespace geoserver.net.Models.Wmts;

/// <summary>
/// Request payload for creating or updating a WMTS store.
/// </summary>
public sealed class WmtsStoreCreateRequest
{
    /// <summary>
    /// Store name.
    /// </summary>
    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Remote WMTS capabilities URL.
    /// </summary>
    [JsonProperty("capabilitiesURL")]
    public string? CapabilitiesUrl { get; set; }
}
