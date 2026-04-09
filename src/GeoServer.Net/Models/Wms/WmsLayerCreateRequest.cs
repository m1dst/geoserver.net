using Newtonsoft.Json;

namespace geoserver.net.Models.Wms;

/// <summary>
/// Request payload for publishing/updating a WMS layer.
/// </summary>
public sealed class WmsLayerCreateRequest
{
    /// <summary>
    /// Layer name.
    /// </summary>
    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Native remote layer name.
    /// </summary>
    [JsonProperty("nativeName")]
    public string? NativeName { get; set; }
}
