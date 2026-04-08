using Newtonsoft.Json;

namespace geoserver.net.Models.LayerGroups;

/// <summary>
/// Single layer group response wrapper.
/// </summary>
public sealed class LayerGroupResponse
{
    /// <summary>
    /// Layer group object.
    /// </summary>
    [JsonProperty("layerGroup")]
    public LayerGroupDto LayerGroup { get; set; } = new();
}
