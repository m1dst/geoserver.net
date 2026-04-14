using Newtonsoft.Json;

namespace GeoServer.Models.Layers;

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
