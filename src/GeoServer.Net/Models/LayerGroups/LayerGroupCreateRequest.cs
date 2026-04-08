using Newtonsoft.Json;

namespace geoserver.net.Models.LayerGroups;

/// <summary>
/// Request payload used to create a layer group.
/// </summary>
public sealed class LayerGroupCreateRequest
{
    /// <summary>
    /// Layer group name.
    /// </summary>
    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Optional display mode.
    /// </summary>
    [JsonProperty("mode")]
    public string? Mode { get; set; }
}
