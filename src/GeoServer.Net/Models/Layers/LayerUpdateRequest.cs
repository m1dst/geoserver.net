using Newtonsoft.Json;

namespace geoserver.net.Models.Layers;

/// <summary>
/// Request payload for updating a layer.
/// </summary>
public sealed class LayerUpdateRequest
{
    /// <summary>
    /// Enabled flag.
    /// </summary>
    [JsonProperty("enabled")]
    public bool? Enabled { get; set; }

    /// <summary>
    /// Default style to apply.
    /// </summary>
    [JsonProperty("defaultStyle")]
    public LayerStyleReferenceDto? DefaultStyle { get; set; }
}
