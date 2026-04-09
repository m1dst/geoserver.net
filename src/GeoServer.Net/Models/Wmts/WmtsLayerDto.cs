using Newtonsoft.Json;

namespace geoserver.net.Models.Wmts;

/// <summary>
/// WMTS layer representation.
/// </summary>
public sealed class WmtsLayerDto
{
    /// <summary>
    /// Layer name.
    /// </summary>
    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;
}
