using Newtonsoft.Json;

namespace geoserver.net.Models.Layers;

/// <summary>
/// Layer style reference payload.
/// </summary>
public sealed class LayerStyleReferenceDto
{
    /// <summary>
    /// Style name.
    /// </summary>
    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;
}
