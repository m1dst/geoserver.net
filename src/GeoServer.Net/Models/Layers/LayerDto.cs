using Newtonsoft.Json;

namespace GeoServer.Models.Layers;

/// <summary>
/// Layer representation.
/// </summary>
public sealed class LayerDto
{
    /// <summary>
    /// Layer name.
    /// </summary>
    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Optional enabled flag.
    /// </summary>
    [JsonProperty("enabled")]
    public bool? Enabled { get; set; }
}
