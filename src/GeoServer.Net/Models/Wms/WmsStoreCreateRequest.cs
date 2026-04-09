using Newtonsoft.Json;

namespace geoserver.net.Models.Wms;

/// <summary>
/// Request payload for creating or updating a WMS store.
/// </summary>
public sealed class WmsStoreCreateRequest
{
    /// <summary>
    /// Store name.
    /// </summary>
    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Remote WMS capabilities URL.
    /// </summary>
    [JsonProperty("capabilitiesURL")]
    public string? CapabilitiesUrl { get; set; }
}
