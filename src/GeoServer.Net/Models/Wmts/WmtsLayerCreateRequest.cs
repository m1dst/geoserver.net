using Newtonsoft.Json;

namespace geoserver.net.Models.Wmts;

/// <summary>
/// Request payload for publishing/updating a WMTS layer.
/// </summary>
public sealed class WmtsLayerCreateRequest
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
