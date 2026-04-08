using Newtonsoft.Json;

namespace geoserver.net.Models.LayerGroups;

/// <summary>
/// Layer group representation.
/// </summary>
public sealed class LayerGroupDto
{
    /// <summary>
    /// Layer group name.
    /// </summary>
    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;
}
