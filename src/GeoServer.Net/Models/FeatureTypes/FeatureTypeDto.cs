using Newtonsoft.Json;

namespace GeoServer.Models.FeatureTypes;

/// <summary>
/// Feature type representation.
/// </summary>
public sealed class FeatureTypeDto
{
    /// <summary>
    /// Feature type name.
    /// </summary>
    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Human-readable title.
    /// </summary>
    [JsonProperty("title")]
    public string? Title { get; set; }
}
