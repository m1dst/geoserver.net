using Newtonsoft.Json;

namespace GeoServer.Models.Layers;

/// <summary>
/// Single layer response wrapper.
/// </summary>
public sealed class LayerResponse
{
    /// <summary>
    /// Layer object.
    /// </summary>
    [JsonProperty("layer")]
    public LayerDto Layer { get; set; } = new();
}
